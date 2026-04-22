using horse_kurs.Interfaces;
using horse_kurs.Models;
using horse_kurs.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IEquestrianService, EquestrianService>();
builder.Services.AddScoped<ExcelExportService>();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<EquestrianClubContext>(options =>
    options.UseSqlServer(connectionString));

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
    });
});
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader();
    });
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var context = scope.ServiceProvider.GetRequiredService<EquestrianClubContext>();
    try
    {
        context.Database.Migrate();
        Console.WriteLine("Миграции успешно применены!");

        if (!context.AppUsers.Any(u => u.Login == "admin"))
        {
            context.AppUsers.Add(new AppUser { Login = "admin", Password = "admin123", Role = "Admin" });
            context.SaveChanges();
            Console.WriteLine("Создан админ: admin/admin123");
        }
    }
    catch (Exception ex)
    {
        Console.WriteLine($" Ошибка БД: {ex.Message}");
    }
}



if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

var frontendPath = Path.Combine(Directory.GetCurrentDirectory(), "frontend");
if (!Directory.Exists(frontendPath)) Directory.CreateDirectory(frontendPath);

var fileProvider = new PhysicalFileProvider(frontendPath);

app.UseDefaultFiles(new DefaultFilesOptions { FileProvider = fileProvider });
app.UseStaticFiles(new StaticFileOptions { FileProvider = fileProvider });

app.UseRouting();
app.UseCors();
app.UseAuthorization();

app.MapControllers();

app.MapFallbackToFile("index.html", new StaticFileOptions { FileProvider = fileProvider });

app.Run();