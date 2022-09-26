namespace Assignment3.Entities;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; } = "Anonymous User";
    public string Email { get; set; } = null!;
    public ICollection<Task> Tasks { get; set; } = new HashSet<Task>();
}
