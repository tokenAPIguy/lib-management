using LBMS_API.Data;
using Microsoft.EntityFrameworkCore;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddOpenApi();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("DefaultConnection")));
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment()) {
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();

app.MapGet("/api/v1", () => "Hello World!");

app.MapGet("/api/v1/categories", async (ApplicationDbContext db) => {
    var categories = await db.Categories.ToListAsync();
    return Results.Ok(categories);
});

app.MapGet("/api/v1/categories/{category}", async (string category, ApplicationDbContext db) => {
    var categories = await db.Categories.Where(c => c.Name == category).ToListAsync();
    return Results.Ok(categories);
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary) {
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}