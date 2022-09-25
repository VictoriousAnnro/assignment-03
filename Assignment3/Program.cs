var factory = new KanbanContextFactory();
using var context = factory.CreateDbContext(args);

var tag = new Tag
{
    Name = "Test Tag"
};

// context.Tags.Add(tag);
// context.SaveChanges();

var test = context.Tags.Find(2).Name;

Console.WriteLine(test);
