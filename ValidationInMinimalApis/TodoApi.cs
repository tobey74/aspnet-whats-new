using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http.HttpResults;

public static class TodoApi
{
    static Todo[] sampleTodos =
        [
            new(1, "Walk the dog"),
            new(2, "Do the dishes", DateOnly.FromDateTime(DateTime.Now)),
            new(3, "Do the laundry", DateOnly.FromDateTime(DateTime.Now.AddDays(1))),
            new(4, "Clean the bathroom"),
            new(5, "Clean the car", DateOnly.FromDateTime(DateTime.Now.AddDays(2)))
        ];
    public static RouteGroupBuilder MapTodosApi(this WebApplication app)
    {
        var todosApi = app.MapGroup("/todos");

        todosApi.MapGet("/", () => sampleTodos)
                .WithName("ListTodos");

        todosApi.MapGet("/{id}", Results<Ok<Todo>, NotFound> (int id) =>
            sampleTodos.FirstOrDefault(a => a.Id == id) is { } todo
                ? TypedResults.Ok(todo)
                : TypedResults.NotFound())
            .WithName("GetTodo");

        todosApi.MapPut("/{id}", Results<Ok<Todo>,BadRequest> (int id, Todo body) =>
            {
                if (id != body.Id)
                    return TypedResults.BadRequest();

                var todo = sampleTodos.FirstOrDefault(a => a.Id == id);
                if (todo is null)
                    sampleTodos = sampleTodos.Append(body).ToArray();
                else
                    todo = todo with { Title = body.Title, DueBy = body.DueBy, Description = body.Description, Category = body.Category, Priority = body.Priority };
                return TypedResults.Ok(todo);
            })
            .WithName("CreateOrReplaceTodo");

        return todosApi;
    }
}

public record Todo(
    int Id,

    [property: Required]
    [property: StringLength(100, MinimumLength = 3)]
    string Title,

    [DataType(DataType.Date)]
    DateOnly? DueBy = null,

    [property: StringLength(500, ErrorMessage = "Description cannot exceed 500 characters")]
    string? Description = null,

    [StringLength(50, ErrorMessage = "Category cannot exceed 50 characters")]
    string Category = "General",

    [property: Range(1, 5, ErrorMessage = "Priority must be between 1 and 5")]
    int Priority = 1
);
