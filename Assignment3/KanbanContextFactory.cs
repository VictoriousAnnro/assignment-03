using Microsoft.EntityFrameworkCore;
using Assignment3.Entities;
using Microsoft.EntityFrameworkCore.Design;

namespace Assignment3;

internal class KanbanContextFactory : IDesignTimeDbContextFactory<KanbanContext>
{
    public KanbanContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var connectionString = configuration.GetConnectionString("Kanban");

        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        optionsBuilder.UseNpgsql(connectionString);

        var context = new KanbanContext(optionsBuilder.Options);
        /* context.Destroy(); */
        /* Seed(context); */
        return context;
    }

    private static void Seed(KanbanContext context)
    {
        var testTag = new Tag { Name = "test" };

        var testUser = new User { Email = "test@itu.dk", Name = "TestUser" };

        var testTask = new Assignment3.Entities.Task { Title = "Do Stuff", AssignedTo = testUser, Description = "Do Some Stuff!!", State = State.New, Tags = new[] { testTag } };

        context.Tasks.Add(testTask);

        context.SaveChanges();
    }
}
