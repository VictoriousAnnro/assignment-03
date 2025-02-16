namespace Assignment3.Entities;

public class Tag
{
    public int Id { get; set; }

    [Required]
    [StringLength(50)]
    public string Name { get; set; } = "Unnamed Tag";

    public ICollection<Task> Tasks { get; set; } = new HashSet<Task>();
}
