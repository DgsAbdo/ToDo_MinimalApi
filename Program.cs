using MiniToDo.Data;
using MiniToDo.ViewModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();

var app = builder.Build();

app.MapGet("v1/Todos", (AppDbContext context) => {
    var toDos = context.ToDos.ToList();
    return Results.Ok(toDos);
});

app.MapGet("v1/Todos/{id}", (AppDbContext context, Guid id) =>
{
    var todo = context.ToDos.Find(id);

    return Results.Created($"/v1/Todos/{id}", todo);
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

app.MapPut("v1/Todos/{id}", (AppDbContext context, Guid id, Todo input) =>
{
    var todo = context.ToDos.Find(id);
    if (todo == null)
        return Results.NotFound();

    todo.Done = input.Done;

    context.ToDos.Update(todo);
    context.SaveChanges();

    return Results.Created($"/v1/Todos/{todo.Id}", todo);
});

app.MapDelete("v1/Todos/{id}", (AppDbContext context, Guid id) => 
{
    var todo = context.ToDos.Find(id);
    
    if(todo != null)
    {
        context.Remove(id);
        context.SaveChanges();
        return Results.Ok();
    }

    return Results.BadRequest();
});

app.Run();
