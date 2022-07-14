public class Todo
{
    public Todo(Guid id, string title, bool done)
    {
        Id = id;
        Title = title;
        Done = done;
    }

    public Guid Id { get; set; }
    public string Title { get; set; }
    public bool Done { get; set; }
}


