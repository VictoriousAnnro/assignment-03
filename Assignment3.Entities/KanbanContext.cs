namespace Assignment3.Entities;

public partial class KanbanContext : DbContext
{
    public KanbanContext(DbContextOptions<KanbanContext> options)
        : base(options)
    { }

    public virtual DbSet<Task> Tasks { get; set; } = null!;
    public virtual DbSet<User> Users { get; set; } = null!;
    public virtual DbSet<Tag> Tags { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Task>(entity =>
        {
            entity.Property(e => e.Title).HasMaxLength(100).IsRequired();
            entity.Property(e => e.State).IsRequired();
            entity.Property(e => e.State).HasConversion<string>();
            //many to many ref missing
            //like this? V
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(100).IsRequired();

            entity.Property(e => e.Email).HasMaxLength(100).IsRequired(); //unique

            entity.HasIndex(e => e.Email).IsUnique();

        });

        modelBuilder.Entity<Tag>(entity =>
        {
            entity.Property(e => e.Name).HasMaxLength(50).IsRequired(); //unique

            entity.HasIndex(e => e.Name).IsUnique();

            //many to many ref missing
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);

    public void Clear()
    {
        using var transaction = this.Database.BeginTransaction();
        Users.RemoveRange(Users);
        Tasks.RemoveRange(Tasks);
        Tags.RemoveRange(Tags);
        SaveChanges();
        transaction.Commit();
    }
}
