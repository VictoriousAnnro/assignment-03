namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private KanbanContext _context;

    public TaskRepository(KanbanContext context)
    {
        _context = context;
    }

    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var assignedUser = task.AssignedToId != null ? _context.Users.Find(task.AssignedToId) : null!;
        if (assignedUser == null && task.AssignedToId != null) return (Response.BadRequest, -1);
        var tags = task.Tags.Select(t => new Tag { Name = t }).ToHashSet();

        var newTask = new Task
        {
            Title = task.Title,
            AssignedTo = assignedUser,
            Description = task.Description,
            State = State.New,
            Tags = tags,
            Created = DateTime.UtcNow,
            StateUpdated = DateTime.UtcNow
        };

        _context.Tasks.Add(newTask);
        _context.SaveChanges();

        return (Response.Created, newTask.Id);
    }

    public Response Delete(int taskId)
    {
        throw new NotImplementedException();
    }

    public TaskDetailsDTO Read(int taskId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        throw new NotImplementedException();
    }

    public Response Update(TaskUpdateDTO task)
    {
        throw new NotImplementedException();
    }
}
