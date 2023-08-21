
using DCBotDataBase.Engine;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

var builder = new ConfigurationBuilder();
builder.SetBasePath(Directory.GetCurrentDirectory());
builder.AddJsonFile("cfgApp.json");
var config = builder.Build();

string connectionString = config.GetConnectionString("DefaultConnection");

var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
var options = optionsBuilder.UseSqlite(connectionString).Options;

using (DataContext db = new DataContext(options))
{
    var users = db.Users.ToList();
	foreach (var user in users)
	{
		Console.WriteLine($"Name {user.Name} Age {user.Age}");
	}
}


Console.ReadKey(); 
