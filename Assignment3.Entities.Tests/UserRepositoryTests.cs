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
    [Fact]
    public void Delete_with_the_force_deletes_if_in_use()
    {
        var blockingTask = new Task { Title = "Blocker!", State = State.New };
        var user = new User { Email = "test@itu.dk", Tasks = new HashSet<Task>() { blockingTask } };
        _context.Users.Add(user);
        _context.Tasks.Add(blockingTask);
        _context.SaveChanges();
        var expected = Response.Deleted;

        var actual = _repo.Delete(userId: user.Id, force: true);

        actual.Should().Be(expected);
    }

    [Fact]
    public void Update_updates_information_and_returns_updated()
    {
        var user = new User { Name = "TestUser", Email = "test@itu.dk" };
        _context.Users.Add(user);
        _context.SaveChanges();
        var expected = Response.Updated;

        var actual = _repo.Update(new UserUpdateDTO(user.Id, "ModifiedUser", "ny@email.dk"));

        actual.Should().Be(expected);
        // _context.Users.Find(user.Id).Name.Should().Be("ModifiedUser");
    }


    [Fact]
    public void Update_updates_user_to_have_new_information_correct()
    {
        var user = new User { Name = "Test", Email = "test@itu.dk" };
        _context.Users.Add(user);
        _context.SaveChanges();

        var expectedName = "newTestUser";
        var expectedEmail = "newEmailTest@itu.dk";

        var actual = _repo.Update(new UserUpdateDTO(user.Id, "newTestUser", "newEmailTest@itu.dk"));

        _context.Users.Find(user.Id).Name.Should().Be(expectedName);
        _context.Users.Find(user.Id).Email.Should().Be(expectedEmail);
    }


    [Fact]
    public void Read_if_no_user_found_returns_null()
    {
        var actual = _repo.Read(6342);
        actual.Should().Be(null);
    }

    [Fact]
    public void Read_returns_correct_info_if_user_found()
    {

        var user = new User { Name = "testUser", Email = "testUser@itu.dk" };
        _context.Users.Add(user);
        _context.SaveChanges();

        var expected = new UserDTO(user.Id, "testUser", "testUser@itu.dk");

        var actual = _repo.Read(user.Id);

        actual.Should().Be(expected);
    }



    [Fact]
    public void ReadAll_should_return_all_elements()
    {

        var user = new User { Name = "testUser", Email = "testUser@itu.dk" };
        _context.Users.Add(user);
        _context.SaveChanges();

        var usersInContext = _context.Users.ToList();

        var usersFromReadAll = _repo.ReadAll();

        var expectedValues = usersInContext.Select(u => (u.Id, u.Name, u.Email));
        var actualValues = usersFromReadAll.Select(dto => (dto.Id, dto.Name, dto.Email));

        actualValues.Should().BeEquivalentTo(expectedValues);
    }


    [Fact]
    public void Update_should_return_notFound_if_user_not_found()
    {
        var userDTO = new UserUpdateDTO(10, "testUser", "testUser@itu.dk");

        var expected = Response.NotFound;

        var actual = _repo.Update(userDTO);

        actual.Should().Be(expected);

    }


    //test stringlength :)



}
