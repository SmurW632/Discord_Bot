using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Discord_Bot_SmurW.ApplicationContext
{
    public class WorkDataBase
    {
        private static DbContextOptions<DataContext>? _optionBuilder;
        public void RunDataBase()
        {
            var builder = new ConfigurationBuilder();
            
            builder.SetBasePath(Directory.GetCurrentDirectory());
            builder.AddJsonFile("dbSettings.json");

            var config = builder.Build();
            string? connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            var options = optionsBuilder.UseSqlite(connectionString).Options;

            _optionBuilder = options;

        }

        public static DbContextOptions<DataContext> OptionsBuider
        {
            get { return _optionBuilder!; }
            set { _optionBuilder = value; }
        }
    }
}
