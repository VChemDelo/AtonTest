using Microsoft.EntityFrameworkCore;

namespace AtonTest.Models;

public partial class AtDbContext : DbContext
{

    
    public AtDbContext(DbContextOptions<AtDbContext> options) 
        :base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(p => p.Id).ValueGeneratedNever();

            entity.Property(p => p.CreatedBy).HasMaxLength(20).IsUnicode(false);

            entity.Property(p => p.Birthday).HasColumnType("datetime");

            entity.Property(p=>p.Login).HasMaxLength(20).IsUnicode(false);

            entity.Property(p=>p.ModifiedBy).HasMaxLength(20).IsUnicode(false);

            entity.Property(p=> p.Name).HasMaxLength(20).IsUnicode(false);

            entity.Property(p=>p.Password).HasMaxLength(30).IsUnicode(false);

            entity.Property(p=>p.RevorkedBy).HasMaxLength(20).IsUnicode(false);
        });
    }
}
