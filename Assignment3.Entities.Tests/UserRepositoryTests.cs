namespace Assignment3.Entities.Tests;

[Collection("Sequential")]
public class UserRepositoryTests : IDisposable
{
    private KanbanContext _context;

    private UserRepository _repo;

    public UserRepositoryTests()
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<UserRepositoryTests>().Build();
        var connectionString = configuration.GetConnectionString("Kanban");

        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        optionsBuilder.UseNpgsql(connectionString);

        _context = new KanbanContext(optionsBuilder.Options);
        _repo = new UserRepository(_context);
        _context.Database.BeginTransaction();
    }

    public void Dispose()
    {
        _context.Database.RollbackTransaction();
        _context.Dispose();
    }

    [Fact]
    public void Create_when_valid_should_return_created()
    {
        // Arrange
        var newUserDTO = new UserCreateDTO(Name: "TestUser", Email: "test@itu.dk");
        var expected = Response.Created;
        // Act
        var (response, userId) = _repo.Create(newUserDTO);

        // Assert
        response.Should().Be(expected);
    }

    [Fact]
    public void Create_when_email_already_exists_returns_conflict()
    {
        // Arrange
        var conflictingUser = new User { Email = "conflict@itu.dk" };
        _context.Users.Add(conflictingUser);
        _context.SaveChanges();
        var expected = (Response.Conflict, conflictingUser.Id);

        // Act
        var newUserDTO = new UserCreateDTO(Name: "NewUser", Email: "conflict@itu.dk");
        var actual = _repo.Create(newUserDTO);

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void Delete_with_no_force_when_user_in_use_returns_conflict()
    {
        var blockingTask = new Task { Title = "Blocker!", State = State.New };
        var user = new User { Email = "test@itu.dk", Tasks = new HashSet<Task>() { blockingTask } };
        _context.Users.Add(user);
        _context.Tasks.Add(blockingTask);
        _context.SaveChanges();
        var expected = Response.Conflict;

        var actual = _repo.Delete(user.Id);

        actual.Should().Be(expected);
    }

    // delete_with_force_deletes_even_if_in_use

    // Update returns proper response
    // Update correctly updates

    // Read if no user found returns null

    // Read if found returns correct info

    // create, update, delete returns NotFound if not found

    // readall returns all elements

}
