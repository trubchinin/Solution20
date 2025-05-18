using DataAccessLayer;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

var builder = WebApplication.CreateBuilder(args);

// 1) Налаштовуємо CORS, щоб Blazor-клієнт (port 7236) міг робити запити на API (7086)
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy
            .WithOrigins("https://localhost:7236") // адреса твого LibraryClient
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

// 2) Додаємо EF Core з SQLite
builder.Services.AddDbContext<LibraryContext>(options =>
    options.UseSqlite("Data Source=library.db"));

// 3) Реєструємо наш репозиторій
builder.Services.AddScoped<LibraryRepository>();

// 4) Додаємо підтримку контролерів
builder.Services.AddControllers();

// 5) Додаємо Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo
    {
        Title = "LibraryApi",
        Version = "v1"
    });
});

var app = builder.Build();

// --- 6) На старті створюємо БД і таблиці, якщо їх ще нема ---
using (var scope = app.Services.CreateScope())
{
    var ctx = scope.ServiceProvider.GetRequiredService<LibraryContext>();
    ctx.Database.EnsureCreated();
}

// 7) Підключаємо CORS middleware
app.UseCors();

// 8) Swagger у Development
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "LibraryApi v1");
    });
}

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();

app.Run();