namespace Assignment3.Entities;

public class User
{
    public int Id {get; set;}

    [MaxLength(100)]
    [Required]
    public string Name {get; set;}

    [MaxLength(100)]
    [Required]
    public string Email {get; set;} //unique in contect
    public ICollection<Task> Tasks {get; set;}
}
