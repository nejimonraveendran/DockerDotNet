using dal.Config;
using dal.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace dal
{
    public class MyAppDbContext : DbContext
    {
        public MyAppDbContext()
        {

        }

        public MyAppDbContext(DbContextOptions dbContextOptions) : base(dbContextOptions)
        {

        }

        public DbSet<User>? Users { get; set; } 
        public DbSet<Product>? Products { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new UserConfig());
            modelBuilder.ApplyConfiguration(new ProductConfig());
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                var config = new ConfigurationBuilder()
                    .SetBasePath(Directory.GetCurrentDirectory())
                    .AddJsonFile("dbmigration.json")
                    .Build();

                //try to get connection string from environment (when run through Docker Compose)
                var connectionString = Environment.GetEnvironmentVariable("DBCONNECTION");

                //when run directly from VS Code
                if (string.IsNullOrEmpty(connectionString))
                {
                    connectionString = config.GetConnectionString("MyAppDb");
                }

                Console.WriteLine(connectionString);

                optionsBuilder.UseSqlServer(connectionString);
            }
        }


    }
}