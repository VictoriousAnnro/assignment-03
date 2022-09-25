using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Assignment3.Entities;

var factory = new KanbanContextFactory();
using var context = factory.CreateDbContext(args);

var user = new User {
    Id = 1,
    Name = "testUser",
    Email = "lol@mail.com",
    Tasks = null
};

context.Users.Add(user);
context.SaveChanges();

var harley = context.Users.Find(1);

Console.WriteLine(harley);



/*var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();
var connectionString = configuration.GetConnectionString("SQLServer");

var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
optionsBuilder.UseSqlServer(connectionString);

var options = optionsBuilder.Options;


//var input = Console.ReadLine();

using var context = new KanbanContext(options);

Console.WriteLine(context.Database.CanConnect());

/*var user = new User {
    Id = 1,
    Name = "testUser",
    Email = "lol@mail.com",
    Tasks = null
};

context.Users.Add(user);
context.SaveChanges();

var harley = context.Users.Find(1);

Console.WriteLine(harley);*/
