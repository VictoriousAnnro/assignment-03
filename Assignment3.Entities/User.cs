namespace Assignment3.Entities;

public class User
{
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Name { get; set; } = "Anonymous User";

    [Required]
    [StringLength(100)]
    [EmailAddress]
    public string Email { get; set; } = null!;

    public ICollection<Task> Tasks { get; set; } = new HashSet<Task>();
}
