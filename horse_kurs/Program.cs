using horse_kurs.Models;
using horse_kurs.Interfaces;
using horse_kurs.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EquestrianClubContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddScoped<IHorseService, HorseService>();
builder.Services.AddScoped<ILessonService, LessonService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<ICompetitionService, CompetitionService>();

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = "horse-kurs",
            ValidAudience = "horse-kurs-client",
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes("your-secret-key-must-be-at-least-32-characters-long-here!"))
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
    options.AddPolicy("CoachOnly", policy => policy.RequireRole("coach"));
    options.AddPolicy("ClientOnly", policy => policy.RequireRole("client"));
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", b => b.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader());
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EquestrianClubContext>();
    try
    {
        Console.WriteLine("Проверка базы данных...");
        await context.Database.EnsureCreatedAsync();

        if (!await context.Horses.AnyAsync())
        {
            Console.WriteLine("База пуста. Запуск SQL-скрипта инициализации...");
            await InitializeDatabaseFromScript(context);
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при подготовке БД: {ex.Message}");
    }
}

var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "frontend");
if (!Directory.Exists(frontendPath)) Directory.CreateDirectory(frontendPath);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

//app.UseHttpsRedirection();
app.UseCors("AllowAll");

app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = new PhysicalFileProvider(frontendPath) });
app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(frontendPath) });

app.UseAuthentication();
app.UseAuthorization();


// Маппинг контроллеров API
app.MapControllers();

app.MapFallbackToFile("index.html", new StaticFileOptions { FileProvider = new PhysicalFileProvider(frontendPath) });

Console.WriteLine("Приложение Equestrian Club успешно запущено!");
await app.RunAsync();

async Task InitializeDatabaseFromScript(EquestrianClubContext context)
{
    try
    {
        string sqlScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "Database", "CreateDatabase.sql");
        if (File.Exists(sqlScriptPath))
        {
            string sqlScript = await File.ReadAllTextAsync(sqlScriptPath);
            var commands = sqlScript.Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(cmd => cmd.Trim())
                .Where(cmd => !string.IsNullOrWhiteSpace(cmd) && !cmd.StartsWith("USE "));

            foreach (var command in commands)
            {
                try { await context.Database.ExecuteSqlRawAsync(command); }
                catch (Exception ex) { Console.WriteLine($"Ошибка в команде скрипта: {ex.Message}"); }
            }
            Console.WriteLine("Данные из SQL скрипта успешно импортированы.");
        }
        else
        {
            Console.WriteLine("Файл скрипта CreateDatabase.sql не найден.");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Критическая ошибка выполнения скрипта: {ex.Message}");
    }
}