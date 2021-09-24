using Microsoft.EntityFrameworkCore;
using Worms.database.entities;

namespace Worms.database
{
    public class DatabaseContext : DbContext
    {
        private readonly string _connectionString;

        public DatabaseContext(string connectionString = null)
        {
            _connectionString = connectionString;
            if (connectionString != null)
                Database.EnsureCreated();
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (_connectionString == null)
                optionsBuilder.UseInMemoryDatabase("WormsBase");
            else
            {
                optionsBuilder.UseSqlServer(_connectionString);
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<WorldBehaviour>().OwnsMany(
                worldBehaviour => worldBehaviour.FoodPoints, 
                navigationBuilder => 
                { navigationBuilder.WithOwner().HasForeignKey("BehaviourId"); });
                
                modelBuilder.Entity<WorldBehaviour>()
                    .HasIndex(behaviour => behaviour.Name)
                    .IsUnique();
        }

        public DbSet<WorldBehaviour> WorldBehaviours { get; set; }
    }
}