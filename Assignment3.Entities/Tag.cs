namespace Assignment3.Entities;

public class Tag
{
    public int Id { get; set; }
    public string Name { get; set; } = "Unnamed Tag";
    public ICollection<Task> Tasks { get; set; } = new List<Task>();
    public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
}
