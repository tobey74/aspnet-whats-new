using System.Text.Json.Nodes;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi(options =>
{
    // Specify the OpenAPI version to use.
    // options.OpenApiVersion = Microsoft.OpenApi.OpenApiSpecVersion.OpenApi3_0;
    // Here's how to add an example in .NET 9
    // options.AddSchemaTransformer((schema, context, cancellationToken) =>
    // {
    //     if (context.JsonTypeInfo.Type == typeof(WeatherForecast))
    //     {
    //         schema.Example = new OpenApiObject
    //         {
    //             ["date"] = new OpenApiString(DateTime.Now.AddDays(1).ToString("yyyy-MM-dd")),
    //             ["temperatureC"] = new OpenApiInteger(0),
    //             ["temperatureF"] = new OpenApiInteger(32),
    //             ["summary"] = new OpenApiString("Bracing"),
    //         };
    //     }
    //     return Task.CompletedTask;
    // });
    options.AddSchemaTransformer((schema, context, cancellationToken) =>
    {
        if (context.JsonTypeInfo.Type == typeof(WeatherForecast))
        {
            schema.Example = new JsonObject
            {
                ["date"] = DateTime.Now.AddDays(1).ToString("yyyy-MM-dd"),
                ["temperatureC"] = 0,
                ["temperatureF"] = 32,
                ["summary"] = "Bracing",
            };
        }
        return Task.CompletedTask;
    });
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

var summaries = new[]
{
    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
};

app.MapGet("/weatherforecast", () =>
{
    var forecast =  Enumerable.Range(1, 5).Select(index =>
        new WeatherForecast
        (
            DateOnly.FromDateTime(DateTime.Now.AddDays(index)),
            Random.Shared.Next(-20, 55),
            summaries[Random.Shared.Next(summaries.Length)]
        ))
        .ToArray();
    return forecast;
})
.WithName("GetWeatherForecast");

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
