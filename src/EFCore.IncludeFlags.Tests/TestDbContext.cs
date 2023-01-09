using Microsoft.EntityFrameworkCore;

namespace EFCore.IncludeFlags.Tests
{
    public class TestDbContext : DbContext
    {
        public DbSet<TestModel>? TestModels { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseInMemoryDatabase(Guid.NewGuid().ToString());
        }
    }
}
