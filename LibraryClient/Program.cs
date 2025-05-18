using LibraryClient;
using LibraryClient.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using System.Net.Http;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

// HttpClient для API
builder.Services.AddScoped(sp =>
    new HttpClient { BaseAddress = new Uri("https://localhost:7086/") });

// Сервіси
builder.Services.AddScoped<ReaderService>();
builder.Services.AddScoped<BookService>();
builder.Services.AddScoped<IssueService>();

await builder.Build().RunAsync();