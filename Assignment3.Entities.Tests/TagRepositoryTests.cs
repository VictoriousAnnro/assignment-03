using Assignment3.Core;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace Assignment3.Entities.Tests;

public class TagRepositoryTests
{
    private KanbanContext _context;
    private TagRepository _repository;

    public TagRepositoryTests()
    {
        var connection = new SqliteConnection("Filename=:memory:");
        connection.Open();
        var builder = new DbContextOptionsBuilder<KanbanContext>();
        builder.UseSqlite(connection);
        var context = new KanbanContext(builder.Options);
        context.Database.EnsureCreated();

        var user1 = new User { Name = "Test1", Email = "test1@itu.dk" };
        var user2 = new User { Name = "Test2", Email = "test2@itu.dk" };

        var task1 = new Task { Title = "test1", AssignedTo = user1, State = State.Active };
        var task2 = new Task { Title = "test2", AssignedTo = user1, State = State.New }; //todo remove user?
        var task3 = new Task { Title = "test3", AssignedTo = user2, State = State.Closed };
        var task4 = new Task { Title = "test4", AssignedTo = user2, State = State.Removed };

        context.Tags.AddRange(
            new Tag { Name = "tag1", Tasks = new List<Task> { task1, task2 } },
            new Tag { Name = "tag2", Tasks = new List<Task> { task3, task4 } },
            new Tag { Name = "tag3", Tasks = new List<Task>() }
        );

        context.SaveChanges();

        _context = context;
        _repository = new TagRepository(context);
    }

    [Fact]
    public void Create_Tag_succeeds()
    {
        var toBeCreated = new TagCreateDTO("tag4");

        var expected = (Response.Created, 4);

        var actual = _repository.Create(toBeCreated);

        Assert.Equal(expected, actual);
    }


}
