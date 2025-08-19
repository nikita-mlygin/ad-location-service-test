using AdLocationService.Domain.AdLocation.Construct;
using AdLocationService.Domain.AdLocation.Repository;
using FastEndpoints;
using Microsoft.AspNetCore.Diagnostics;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Debug()
    .WriteTo.Console() // вывод в консоль
    .WriteTo.File(
        path: "logs/log-.txt",
        rollingInterval: RollingInterval.Day,
        retainedFileCountLimit: 7,
        outputTemplate: "{Timestamp:yyyy-MM-dd HH:mm:ss} [{Level}] {Message}{NewLine}{Exception}"
    )
    .CreateLogger();

builder.Services.AddOpenApi();

builder.Services.AddFastEndpoints();

builder.Services.AddSingleton<ITrieRepository, TrieInMemoryRepository>();
builder.Services.AddSingleton<ILocationTrieBuilder, LocationTrieBuilder>();

builder.Host.UseSerilog();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseFastEndpoints();

app.Run();
