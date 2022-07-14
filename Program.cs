using MiniToDo.Data;
using MiniToDo.ViewModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.MapGet("v1/Todos", (AppDbContext context) => {
    var toDos = context.ToDos.ToList();
    return Results.Ok(toDos);
});

app.MapPost("v1/Todos", (AppDbContext context ,CreateTodoModel model) => 
{
    var todo = model.MapTo();
    if(!model.IsValid)
        return Results.BadRequest(model.Notifications);

    context.ToDos.Add(todo);
    context.SaveChanges();

    return Results.Created($"/v1/Todos/{todo.Id}", todo);
});

app.Run();
