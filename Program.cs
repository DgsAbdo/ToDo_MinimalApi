using MiniToDo.Data;
using MiniToDo.ViewModels;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddDbContext<AppDbContext>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();
app.UseSwagger();
app.UseSwaggerUI();

app.MapGet("v1/Todos", (AppDbContext context) => {
    var toDos = context.ToDos.ToList();
    return Results.Ok(toDos);
});

app.MapGet("v1/ToDos/{id}", (AppDbContext context, Guid id) =>
{
    var todo = context.ToDos.Find(id);
    if(todo == null)
        return Results.NotFound();

    return Results.Created($"/v1/ToDos/{id}", todo);
});

app.MapPost("v1/ToDos", (AppDbContext context ,CreateTodoModel model) => 
{
    var todo = model.MapTo();
    if(!model.IsValid)
        return Results.BadRequest(model.Notifications);

    context.ToDos.Add(todo);
    context.SaveChanges();

    return Results.Created($"/v1/ToDos/{todo.Id}", todo);
});

app.MapPut("v1/ToDos/{id}", (AppDbContext context, Guid id, Todo input) =>
{
    var todo = context.ToDos.Find(id);
    if (todo == null)
        return Results.BadRequest();

    todo.Done = input.Done;

    context.ToDos.Update(todo);
    context.SaveChanges();

    return Results.Created($"/v1/ToDos/{todo.Id}", todo);
});

app.MapDelete("v1/ToDos/{id}", (AppDbContext context, Guid id) => 
{
    var todo = context.ToDos.Find(id);
    
    if(todo == null)
        return Results.BadRequest();

    context.Remove(todo);
    context.SaveChanges();
    return Results.Ok();
});

app.Run();
