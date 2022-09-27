namespace Assignment3.Entities.Tests;

[Collection("Sequential")]
public class TagRepositoryTests : IDisposable
{
    private KanbanContext _context;

    private TagRepository _repo;
    public TagRepositoryTests()
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<TagRepositoryTests>().Build();
        var connectionString = configuration.GetConnectionString("Kanban");

        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        optionsBuilder.UseNpgsql(connectionString);

        _context = new KanbanContext(optionsBuilder.Options);
        _repo = new TagRepository(_context);
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
        var newTagDTO = new TagCreateDTO(Name: "testing");
        var expected = Response.Created;
        // Act
        var (response, tagId) = _repo.Create(newTagDTO);

        // Assert
        response.Should().Be(expected);
    }

    [Fact]
    public void Create_when_tag_already_exists_returns_conflict()
    {
        // Arrange
        var conflictingTag = new Tag { Name = "testing" };
        _context.Tags.Add(conflictingTag);
        _context.SaveChanges();
        var expected = (Response.Conflict, conflictingTag.Id);

        // Act
        var newTagDTO = new TagCreateDTO(Name: "testing");
        var actual = _repo.Create(newTagDTO);

        // Assert
        actual.Should().Be(expected);
    }

    [Fact]
    public void Delete_with_no_force_when_tag_assigned_returns_conflict()
    {
        var blockingTask = new Task { Title = "Blocker!", State = State.New };
        var tag = new Tag { Name = "testing", Tasks = new HashSet<Task>() { blockingTask } };
        _context.Tags.Add(tag);
        _context.Tasks.Add(blockingTask);
        _context.SaveChanges();
        var expected = Response.Conflict;

        var actual = _repo.Delete(tag.Id);

        actual.Should().Be(expected);
    }

    [Fact]
    public void Delete_with_the_force_deletes_if_assigned()
    {
        var blockingTask = new Task { Title = "Blocker!", State = State.New };
        var tag = new Tag { Name = "testing", Tasks = new HashSet<Task>() { blockingTask } };
        _context.Tags.Add(tag);
        _context.Tasks.Add(blockingTask);
        _context.SaveChanges();
        var expected = Response.Deleted;

        var actual = _repo.Delete(tagId: tag.Id, force: true);

        actual.Should().Be(expected);
    }

    [Fact]
    public void Update_updates_information_and_returns_updated()
    {
        var tag = new Tag { Name = "testing" };
        _context.Tags.Add(tag);
        _context.SaveChanges();
        var expected = Response.Updated;

        var actual = _repo.Update(new TagUpdateDTO(tag.Id, "updated_tag"));

        actual.Should().Be(expected);
        // _context.Users.Find(user.Id).Name.Should().Be("ModifiedUser");
    }

    [Fact]
    public void Update_updates_tag_to_have_new_information_correct()
    {
        var tag = new Tag { Name = "testing" };
        _context.Tags.Add(tag);
        _context.SaveChanges();

        var expectedName = "updated_tag";

        var actual = _repo.Update(new TagUpdateDTO(tag.Id, "updated_tag"));

        _context.Tags.Find(tag.Id).Name.Should().Be(expectedName);
    }


    [Fact]
    public void Read_if_no_tag_found_returns_null()
    {
        var actual = _repo.Read(Int32.MaxValue);
        actual.Should().Be(null);
    }

    [Fact]
    public void Read_returns_correct_info_if_user_found()
    {

        var tag = new Tag { Name = "testTag" };
        _context.Tags.Add(tag);
        _context.SaveChanges();

        var expected = new TagDTO(tag.Id, "testTag");

        var actual = _repo.Read(tag.Id);

        actual.Should().Be(expected);
    }



    [Fact]
    public void ReadAll_should_return_all_elements()
    {

        var tag1 = new Tag { Name = "testing" };
        var tag2 = new Tag { Name = "testing2" };
        var tag3 = new Tag { Name = "testing3" };
        _context.Tags.AddRange(new[] { tag1, tag2, tag3 });
        _context.SaveChanges();

        var tagsInContext = _context.Tags.ToList();

        var tagsFromReadAll = _repo.ReadAll();

        var expectedValues = tagsInContext.Select(tag => (tag.Id, tag.Name));
        var actualValues = tagsFromReadAll.Select(dto => (dto.Id, dto.Name));

        actualValues.Should().BeEquivalentTo(expectedValues);
    }

    [Fact]
    public void Update_should_return_notFound_if_user_not_found()
    {
        var tagDTO = new TagUpdateDTO(Int32.MaxValue, "updated_tag");

        var expected = Response.NotFound;

        var actual = _repo.Update(tagDTO);

        actual.Should().Be(expected);
    }

}
