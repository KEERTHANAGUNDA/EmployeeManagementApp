using Microsoft.AspNetCore.Mvc;
using EmployeeAPI2;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers(); //  Register MVC Controllers
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
        policy.AllowAnyOrigin()
              .AllowAnyMethod()
              .AllowAnyHeader());
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseCors("AllowAll");

app.MapControllers(); // ✅ Map attribute-routed controllers (like [Route("api/[controller]")])

// (Optional) Keep this demo endpoint or remove it
//var summaries = new[]
//{
//    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
//};

//app.MapGet("/weatherforecast", (HttpContext httpContext) =>
//{
//    var forecast = Enumerable.Range(1, 5).Select(index =>
//        new WeatherForecast
//        {
//            Date = DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
//            TemperatureC = Random.Shared.Next(-20, 55),
//            Summary = summaries[Random.Shared.Next(summaries.Length)]
//        })
//        .ToArray();
//    return forecast;
//})
//.WithName("GetWeatherForecast")
//.WithOpenApi();

app.Run();
