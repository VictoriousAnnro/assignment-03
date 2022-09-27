using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Assignment3;

internal class KanbanContextFactory : IDesignTimeDbContextFactory<KanbanContext>
{
    public KanbanContext CreateDbContext(string[] args)
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<Program>().Build();
        var connectionString = configuration.GetConnectionString("Assignment3");

        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        optionsBuilder.UseSqlServer(connectionString);

        return new KanbanContext(optionsBuilder.Options);
    }
}