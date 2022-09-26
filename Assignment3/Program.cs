var factory = new KanbanContextFactory();
using var context = factory.CreateDbContext(args);

// context.Tags.Add(tag);
// context.SaveChanges();
