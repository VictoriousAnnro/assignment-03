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
        return context;
    }
}
