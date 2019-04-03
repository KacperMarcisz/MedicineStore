
namespace MedicineStore.API.Data
{
    using Microsoft.EntityFrameworkCore;
    using Models;

    public class DataContext : DbContext
    {
        #region Properties

        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<Image> Images { get; set; }

        #endregion

        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Medicine>()
                .Property(x => x.Description)
                .IsRequired()
                .HasMaxLength(5000);
            modelBuilder.Entity<Medicine>()
                .Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255);
        }
    }
}
