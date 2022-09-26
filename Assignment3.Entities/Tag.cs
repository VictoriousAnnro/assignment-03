namespace Assignment3.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = "Unnamed Tag";
    public ICollection<Task> Tasks { get; set; } = new HashSet<Task>();
    public ICollection<TaskTag> TaskTags { get; set; } = new HashSet<TaskTag>();
}
