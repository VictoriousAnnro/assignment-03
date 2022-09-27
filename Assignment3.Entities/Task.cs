namespace Assignment3.Entities;

public class Task
{

    public int Id { get; set; }

    [MaxLength(100)]
    [Required]
    public string Title { get; set; }
    public User? AssignedTo { get; set; }
    public string? Description { get; set; }

    [Required]
    public State State { get; set; }
    public ICollection<Tag> Tags { get; set; } //= new List<Tag>();
    //public ICollection<TaskTag> TaskTags { get; set; } = new List<TaskTag>();
}
