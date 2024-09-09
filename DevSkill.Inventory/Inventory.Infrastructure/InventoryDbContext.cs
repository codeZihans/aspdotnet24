using DevSkill.Inventory.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace DevSkill.Inventory.Infrastructure
{
    public class InventoryDbContext: DbContext
    {
        private readonly string _connectionString;
        private readonly string _migrationAssembly;

        public InventoryDbContext(string connectionString, string migrationAssembly)
        {
            _connectionString = connectionString;
            _migrationAssembly = migrationAssembly;
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(_connectionString,
                    x=>x.MigrationsAssembly(_migrationAssembly));
            }
            base.OnConfiguring(optionsBuilder);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Category>().HasData(new Category 
            { 
                Id = new Guid("12CC1430-94EA-4240-AEC2-CB8E8CCC4FDB"),
                Name ="General" 
            });
            base.OnModelCreating(modelBuilder);
        }
        public DbSet<Product> Products {  get; set; }
        public DbSet<Domain.Entities.Category> Categories { get; set; }
    }
}
