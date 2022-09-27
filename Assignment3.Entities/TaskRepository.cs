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
        var task = _context.Tasks.Find(taskId);
        if (task == null) return Response.NotFound;
        if (new[] { State.Resolved, State.Closed, State.Removed }.Contains(task.State)) return Response.Conflict;
        if (task.State == State.Active)
        {
            task.State = State.Removed;
            _context.SaveChanges();

            return Response.Updated;
        }

        _context.Tasks.Remove(task);
        _context.SaveChanges();

        return Response.Deleted;
    }

    public TaskDetailsDTO Read(int taskId)
    {
        throw new NotImplementedException();
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var tasks = _context.Tasks.Select(task => new TaskDTO(task.Id, task.Title, task.AssignedTo.Name, task.Tags.Select(t => t.Name).ToList().AsReadOnly(), task.State));
        return tasks.ToList().AsReadOnly();
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
