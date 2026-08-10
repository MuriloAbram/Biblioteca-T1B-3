using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using T1B_3Library.Domain.Entities;
using T1B_3Library.Infrastructure.Configurations;

namespace T1B_3Library.Infrastructure.Context
{
    public class AppDbContext : IdentityDbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<Book> Books { get; set; } = null!;
        public DbSet<Gender> Genders { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplica todas as configurações IEntityTypeConfiguration<T> do assembly
            modelBuilder.ApplyConfiguration(new BookConfiguration());
            modelBuilder.ApplyConfiguration(new GenderConfiguration());
        }
    }
}