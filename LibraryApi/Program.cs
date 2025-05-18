using DataAccessLayer;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Якщо середовище **не** Testing → підключаємо SQLite
if (!builder.Environment.IsEnvironment("Testing"))
{
    builder.Services.AddDbContext<LibraryContext>(options =>
        options.UseSqlite("Data Source=library.db"));
}

// Репозиторій завжди доступний (але контекст може бути від InMemory у тестах)
builder.Services.AddScoped<LibraryRepository>();

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// У середовищі розробки показуємо Swagger UI
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Якщо знадобиться авторизація — можна додати app.UseAuthorization();
app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program { }