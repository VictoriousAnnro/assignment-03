using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;


var configuration = new ConfigurationBuilder()
    .AddUserSecrets<Program>()
    .Build();

var connectionString = configuration.GetConnectionString("Assignment3");

var optionsBuilder = new DbContextOptionsBuilder<KanbanContext>();
optionsBuilder.UseSqlServer(connectionString);

var options = optionsBuilder.Options;

using var context = new KanbanContext(options);

var user = new User 
{
    Name = "poul",
    Email = "1234@itu.dk"
};

await context.Users.AddAsync(user);

context.SaveChanges();

var dbuser = context.Users.FirstOrDefault();
   
System.Console.WriteLine(dbuser.Name);
