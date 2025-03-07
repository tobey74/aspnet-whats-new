var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapGet("/hello", Hello);

app.Run();

static partial class Program
{
    /// <summary>
    /// Sends a greeting.
    /// </summary>
    /// <remarks>
    /// Greeting a person by their name.
    /// </remarks>
    /// <param name="name">The name of the person to greet.</param>
    /// <returns>A greeting.</returns>
    public static string Hello(string name)
    {
        return $"Hello, {name}!";
    }
}
