using Microsoft.EntityFrameworkCore;
using Services.ProductService.Core.Models;

namespace Services.ProductService.Data
{
    public class ProductDbContext : DbContext
    {
        private readonly string _connectionString;

        public ProductDbContext()
        {
        }

        public ProductDbContext(string connectionString) : this(GetOptions(connectionString))
        {
            _connectionString = connectionString;
        }

        public ProductDbContext(DbContextOptions options) : base(options)
        {
        }

        private static DbContextOptions GetOptions(string connectionString)
        {
            return new DbContextOptionsBuilder().UseInMemoryDatabase(connectionString).Options;
            //return new DbContextOptionsBuilder().UseSqlServer(connectionString).Options;
        }

        public static ProductDbContext GetProductDbContext(string connectionString)
        {
            return new ProductDbContext(connectionString);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);

            if (_connectionString != null)
                //optionsBuilder.UseSqlServer(_connectionString);
                optionsBuilder.UseInMemoryDatabase(_connectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Id).IsUnique();
                entity.HasIndex(e => new {e.Code, e.Name}).IsUnique(); // unique code and name for each product
                entity.Property(a => a.RowVersion).IsRowVersion();
                entity.Property(a => a.RowVersion).IsConcurrencyToken().ValueGeneratedOnAddOrUpdate();

                entity.Property(e => e.Price).HasColumnType("decimal(10,2)").IsRequired();
                entity.Property(e => e.CreatedAt).HasDefaultValueSql("getdate()");
            });
        }

        public virtual DbSet<Product> Products { get; set; }
    }
}