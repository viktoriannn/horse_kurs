using horse_kurs.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Data.SqlClient;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

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


        options.Events = new JwtBearerEvents
        {
            OnMessageReceived = context =>
            {
                var accessToken = context.Request.Query["access_token"];
                var path = context.HttpContext.Request.Path;

                if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                {
                    context.Token = accessToken;
                }

                return Task.CompletedTask;
            }
        };
    });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminOnly", policy => policy.RequireRole("admin"));
    options.AddPolicy("CoachOnly", policy => policy.RequireRole("coach"));
    options.AddPolicy("ClientOnly", policy => policy.RequireRole("client"));
    options.AddPolicy("CoachOrAdmin", policy => policy.RequireRole("coach", "admin"));
    options.AddPolicy("AllUsers", policy => policy.RequireRole("admin", "coach", "client"));
});


var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EquestrianClubContext>(options =>
    options.UseSqlServer(connectionString));


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", builder =>
    {
        builder.AllowAnyOrigin()
               .AllowAnyMethod()
               .AllowAnyHeader();
    });

    options.AddPolicy("AllowFrontend", builder =>
    {
        builder.WithOrigins("http://localhost:3000", "http://localhost:5000")
               .AllowAnyMethod()
               .AllowAnyHeader()
               .AllowCredentials();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EquestrianClubContext>();

    try
    {
        Console.WriteLine("Проверка базы данных...");

        await context.Database.EnsureCreatedAsync();
        Console.WriteLine("База данных создана или уже существует");

        bool hasData = await context.Horses.AnyAsync();

        if (!hasData)
        {
            await InitializeDatabaseFromScript(context);
        }

        Console.WriteLine($"=== Статистика базы данных ===");
        Console.WriteLine($"Лошадей: {await context.Horses.CountAsync()}");
        Console.WriteLine($"Клиентов: {await context.Clients.CountAsync()}");
        Console.WriteLine($"Тренеров: {await context.Coaches.CountAsync()}");
        Console.WriteLine($"Занятий: {await context.Lessons.CountAsync()}");
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при инициализации БД: {ex.Message}");
        Console.WriteLine($"Стек: {ex.StackTrace}");
    }
}

var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "frontend");
if (!Directory.Exists(frontendPath))
{
    Directory.CreateDirectory(frontendPath);
    Console.WriteLine($"Создана директория для фронтенда: {frontendPath}");
}
else
{
    Console.WriteLine($"Директория фронтенда существует: {frontendPath}");
}

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseCors("AllowAll"); 

app.UseAuthentication();
app.UseAuthorization();  

app.UseDefaultFiles(new DefaultFilesOptions
{
    FileProvider = new PhysicalFileProvider(frontendPath)
});

app.UseStaticFiles(new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(frontendPath),
    OnPrepareResponse = ctx =>
    {
        ctx.Context.Response.Headers.Append("Cache-Control", "public,max-age=3600");
    }
});

app.MapControllers();

app.MapFallbackToFile("index.html", new StaticFileOptions
{
    FileProvider = new PhysicalFileProvider(frontendPath)
});

Console.WriteLine("Приложение запущено");
await app.RunAsync();

async Task InitializeDatabaseFromScript(EquestrianClubContext context)
{
    try
    {
        string sqlScriptPath = Path.Combine(Directory.GetCurrentDirectory(), "Database", "CreateDatabase.sql");

        if (File.Exists(sqlScriptPath))
        {
            Console.WriteLine($"Найден скрипт: {sqlScriptPath}");
            string sqlScript = await File.ReadAllTextAsync(sqlScriptPath);

            var commands = sqlScript
                .Split(new[] { "GO" }, StringSplitOptions.RemoveEmptyEntries)
                .Select(cmd => cmd.Trim())
                .Where(cmd => !string.IsNullOrWhiteSpace(cmd) &&
                              !cmd.StartsWith("USE ") &&
                              !cmd.Contains("DBCC CHECKIDENT"))
                .ToList();

            Console.WriteLine($"Выполняем {commands.Count} команд...");

            int success = 0;
            foreach (var command in commands)
            {
                try
                {
                    await context.Database.ExecuteSqlRawAsync(command);
                    success++;
                }
                catch (Exception ex)
                {
                    if (!ex.Message.Contains("already exists") &&
                        !ex.Message.Contains("There is already an object"))
                    {
                        Console.WriteLine($"Ошибка команды: {ex.Message}");
                        Console.WriteLine($"Команда: {command.Substring(0, Math.Min(100, command.Length))}...");
                    }
                }
            }

            Console.WriteLine($"Выполнено {success} из {commands.Count} команд");
        }
        else
        {
            Console.WriteLine($"Скрипт не найден: {sqlScriptPath}");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($"Ошибка при выполнении скрипта: {ex.Message}");
    }
}