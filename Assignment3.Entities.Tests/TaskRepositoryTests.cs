namespace Assignment3.Entities.Tests;

[Collection("Sequential")]
public class TaskRepositoryTests : IDisposable
{
    private KanbanContext _context;

    private TaskRepository _repo;

    public TaskRepositoryTests()
    {
        var configuration = new ConfigurationBuilder().AddUserSecrets<TaskRepositoryTests>().Build();
        var connectionString = configuration.GetConnectionString("Kanban");

        var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
        optionsBuilder.UseNpgsql(connectionString);

        _context = new KanbanContext(optionsBuilder.Options);
        _repo = new TaskRepository(_context);
        _context.Database.BeginTransaction();
    }

    public void Dispose()
    {
        _context.Database.RollbackTransaction();
        _context.Dispose();
    }

    [Fact]
    public void Delete_a_task_with_state_new_return_deleted()
    {
        var task = new Task { Title = "testTask", State = State.New };

        _context.Tasks.Add(task);
        _context.SaveChanges();

        var expected = Response.Deleted;

        var actual = _repo.Delete(task.Id);

        actual.Should().Be(expected);
    }

    [Fact]
    public void Delete_a_task_with_state_active_should_set_state_to_removed()
    {
        var task = new Task { Title = "testTask", State = State.Active };

        _context.Tasks.Add(task);
        _context.SaveChanges();

        var expectedState = State.Removed;

        _repo.Delete(task.Id);

        _context.Tasks.Find(task.Id).State.Should().Be(expectedState);
    }

    [Fact]
    public void Delete_a_task_with_state_resolved_closed_removed_returns_conflict()
    {
        var task = new Task { Title = "testTask", State = State.Resolved };
        var task1 = new Task { Title = "testTask", State = State.Closed };
        var task2 = new Task { Title = "testTask", State = State.Removed };

        _context.Tasks.Add(task);
        _context.Tasks.Add(task1);
        _context.Tasks.Add(task2);
        _context.SaveChanges();

        var expected = Response.Conflict;

        var actualTask = _repo.Delete(task.Id);
        var actualTask1 = _repo.Delete(task1.Id);
        var actualTask2 = _repo.Delete(task2.Id);

        actualTask.Should().Be(expected);
        actualTask1.Should().Be(expected);
        actualTask2.Should().Be(expected);
    }


    [Fact]
    public void Create_should_set_state_of_new_task_to_New()
    {
        var taskDTO = new TaskCreateDTO("testTask", null, null, new HashSet<string>());
        var expected = State.New;

        _repo.Create(taskDTO);

        var actual = _context.Tasks.OrderBy(t => t.Id).Last().State;

        actual.Should().Be(expected);
    }


    [Fact]
    public void Create_should_set_created_and_stateupdated_to_currenttime()
    {
        var taskDTO = new TaskCreateDTO("testTask", null, null, new HashSet<string>());

        _repo.Create(taskDTO);

        var expected = DateTime.UtcNow;
        var actual = _context.Tasks.OrderBy(t => t.Id).Last().Created;
        var actual1 = _context.Tasks.OrderBy(t => t.Id).Last().StateUpdated;

        actual.Should().BeCloseTo(expected, precision: TimeSpan.FromSeconds(5));
        actual1.Should().BeCloseTo(expected, precision: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Update_allows_to_edit_tags()
    {
        var task = new Task { Title = "testTask" };
        _context.Tasks.Add(task);
        _context.SaveChanges();

        var taskUpdateDto = new TaskUpdateDTO(task.Id, "tesdtUpdateDTO", null, null, new List<string> { "test", "tester" }, State.New);

        var expected = new List<string> { "test", "tester" };

        _repo.Update(taskUpdateDto);

        var actual = _context.Tasks.Find(task.Id).Tags.Select(t => t.Name);

        actual.Should().BeEquivalentTo(expected);
    }

    //maybe do the same for create if time (above)

    [Fact]
    public void Update_changes_stateupdated_to_currenttime()
    {
        var task = new Task { Title = "testTask" };
        _context.Tasks.Add(task);
        _context.SaveChanges();

        var taskUpdateDto = new TaskUpdateDTO(task.Id, "tesdtUpdateDTO", null, null, new List<string> { "test", "tester" }, State.New);

        var expected = DateTime.UtcNow;

        _repo.Update(taskUpdateDto);

        var actual = _context.Tasks.Find(task.Id).StateUpdated;

        actual.Should().BeCloseTo(expected, precision: TimeSpan.FromSeconds(5));
    }

    [Fact]
    public void Create_returns_badrequest_if_assignto_user_dows_not_exist()
    {
        var taskDTO = new TaskCreateDTO("testTask", -1, null, new HashSet<string>());

        var expected = (Response.BadRequest, -1);

        var actual = _repo.Create(taskDTO);

        actual.Should().Be(expected);

    }

    [Fact]
    public void ReadAll_should_return_all_elements()
    {
        var task = new Task {Title = "testTitle"};
        _context.Tasks.Add(task);
        _context.SaveChanges();

        var tasksInContext = _context.Tasks.ToList();

        var tasksFromReadAll = _repo.ReadAll();

        var expectedValues = tasksInContext.Select(t => t.Title);
        var actualValues = tasksFromReadAll.Select(t => t.Title);

        actualValues.Should().BeEquivalentTo(expectedValues);
    }

    [Fact]
    public void ReadAllByTag_should_return_all_elements_with_given_tag()
    {
        var testTag1 = new Tag {Name = "test"};
        var testTag2 = new Tag {Name = "test2"};
        var testTag3 = new Tag {Name = "test3"};
        
        var task1 = new Task {Title = "testTitle", Tags = new HashSet<Tag>(new Tag[] { testTag1, testTag3} )};
        var task2 = new Task {Title = "testTitle2", Tags = new HashSet<Tag>(new Tag[] { testTag2, testTag3} )};
        var task3 = new Task {Title = "testTitle3", Tags = new HashSet<Tag>(new Tag[] { testTag1, testTag2} )};
        _context.Tasks.Add(task1);
        _context.Tasks.Add(task2);
        _context.Tasks.Add(task3);
        _context.SaveChanges();

        var tasksWithTagsFromContext = _context.Tasks.OrderBy(t => t.Id).Reverse().Take(3).ToList();
        var taskIdsWithTag1 = new [] { tasksWithTagsFromContext[0].Id, tasksWithTagsFromContext[2].Id };
        var taskIdsWithTag2 = new [] { tasksWithTagsFromContext[0].Id, tasksWithTagsFromContext[1].Id };
        var taskIdsWithTag3 = new [] { tasksWithTagsFromContext[1].Id, tasksWithTagsFromContext[2].Id };

        var actual1 = _repo.ReadAllByTag("test").Select(t => t.Id);
        var actual2 = _repo.ReadAllByTag("test2").Select(t => t.Id);
        var actual3 = _repo.ReadAllByTag("test3").Select(t => t.Id);

        actual1.Should().BeEquivalentTo(taskIdsWithTag1);
        actual2.Should().BeEquivalentTo(taskIdsWithTag2);
        actual3.Should().BeEquivalentTo(taskIdsWithTag3);
    }
}
