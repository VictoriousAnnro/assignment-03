namespace Assignment3.Entities;

public class TaskRepository : ITaskRepository
{
    private KanbanContext _context;

    public TaskRepository(KanbanContext context)
    {
        _context = context;
    }

    private TaskDTO TaskDTOFromTask(Task task) => new TaskDTO
    (
        Id: task.Id,
        Title: task.Title,
        AssignedToName: task.AssignedTo != null ? task.AssignedTo.Name : "",
        Tags: task.Tags.Select(t => t.Name).ToList().AsReadOnly(),
        State: task.State
    );

    private TaskDetailsDTO TaskDetailsDTOFromTask(Task task) => new TaskDetailsDTO
    (
        Id: task.Id,
        Title: task.Title,
        Description: task.Description != null ? task.Description : "",
        Created: task.Created,
        StateUpdated: task.StateUpdated,
        AssignedToName: task.AssignedTo != null ? task.AssignedTo.Name : "",
        Tags: task.Tags.Select(t => t.Name).ToList().AsReadOnly(),
        State: task.State
    );

    private Tag FindOrCreateTag(string tagName)
    {
        var tagInDB = _context.Tags.Where(t => t.Name.Equals(tagName)).First();
        if(tagInDB != null) return tagInDB;

        return new Tag { Name = tagName};
    }

    public (Response Response, int TaskId) Create(TaskCreateDTO task)
    {
        var assignedUser = task.AssignedToId != null ? _context.Users.Find(task.AssignedToId) : null!;
        if (assignedUser == null && task.AssignedToId != null) return (Response.BadRequest, -1);
        var tags = task.Tags.Select(t => FindOrCreateTag(t)).ToHashSet();

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
        var task = _context.Tasks.Find(taskId);
        return task != null ? TaskDetailsDTOFromTask(task) : null!;
   
        // find in context.tasks, create DTO with fields
        //maybe
    }

    public IReadOnlyCollection<TaskDTO> ReadAll()
    {
        var taskDTOs = _context.Tasks.Select(task => TaskDTOFromTask(task));
        return taskDTOs.ToList().AsReadOnly();
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByState(State state)
    {
        var tasksWithState = _context.Tasks
                            .Where(task => task.State == state)
                            .Select(task => TaskDTOFromTask(task)); 
        return tasksWithState.ToList().AsReadOnly();
        // like above but chain .Where 
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByTag(string tag)
    {
        //find all tasks where the list of tags contain the specified tag
   
        var taskDTOs = _context.Tasks.Select(task => TaskDTOFromTask(task)) 
                                     .Where(task => task.Tags.Contains(tag))
                                     .ToList()
                                     .AsReadOnly();

        return taskDTOs;
    }

    public IReadOnlyCollection<TaskDTO> ReadAllByUser(int userId)
    {
        //find all tasks that are assigned to the specified userId
        var byUser = _context.Users.Find(userId);
        if(byUser == null) return new List<TaskDTO>().AsReadOnly();

        var taskDTOs = _context.Tasks.Where(task => byUser.Id == userId)
                                     .Select(task => TaskDTOFromTask(task))
                                     .ToList()
                                     .AsReadOnly();

        return taskDTOs;

    }

    public IReadOnlyCollection<TaskDTO> ReadAllRemoved()
    {
        //find all tasks with state = Removed
        var tasksWithStateRemoved = _context.Tasks
                            .Where(task => task.State == State.Removed)
                            .Select(task => TaskDTOFromTask(task));
        return tasksWithStateRemoved.ToList().AsReadOnly();;
    }

    public Response Update(TaskUpdateDTO task)
    {
        var curTask = _context.Tasks.Find(task.Id);
        if (curTask == null) return Response.NotFound;

        curTask.Title = task.Title;
        curTask.AssignedTo = _context.Users.Find(task.AssignedToId);
        curTask.Description = task.Description;
        curTask.Tags = task.Tags.Select(name => FindOrCreateTag(name)).ToList();

        if(curTask.State != task.State){
            curTask.State = task.State;
            curTask.StateUpdated = DateTime.UtcNow;
        }
        
        _context.SaveChanges();
        return Response.Updated;
    }
}
