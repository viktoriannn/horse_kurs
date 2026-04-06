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
builder.Services.AddScoped<ExcelExportService>();

// --- Аутентификация и Авторизация ---
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
        Console.WriteLine("Применение миграций базы данных...");
        await context.Database.MigrateAsync();
        Console.WriteLine("База данных успешно обновлена и готова к работе.");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при миграции БД: {ex.Message}");
    }
}

var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "frontend");
if (!Directory.Exists(frontendPath)) Directory.CreateDirectory(frontendPath);

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = new PhysicalFileProvider(frontendPath) });
app.UseStaticFiles(new StaticFileOptions { FileProvider = new PhysicalFileProvider(frontendPath) });

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.MapFallbackToFile("index.html", new StaticFileOptions { FileProvider = new PhysicalFileProvider(frontendPath) });

app.MapGet("/api/excel", async (ExcelExportService service) =>
{
    var fileBytes = await service.GetExcelAsync();
    return Results.File(
        fileBytes,
        "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet",
        $"EquestrianClub_Report_{DateTime.Now:yyyyMMddHHmmss}.xlsx"
    );
});

app.MapControllers();

Console.WriteLine("Приложение Equestrian Club успешно запущено!");
await app.RunAsync();