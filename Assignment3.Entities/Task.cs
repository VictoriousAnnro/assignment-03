namespace Assignment3.Entities;

public class Task
{
    public int Id { get; set; }
    public string Title { get; set; } = "Untitled Task";
    public User? AssignedTo { get; set; }
    public string? Description { get; set; }
    public State State { get; set; }
    public ICollection<Tag> Tags { get; set; } = new HashSet<Tag>();

    public DateTime Created { get; set; }

    public DateTime StateUpdated { get; set; }
}
