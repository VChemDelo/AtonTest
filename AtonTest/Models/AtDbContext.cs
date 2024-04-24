using Microsoft.EntityFrameworkCore;


namespace AtonTest.Models;

public partial class AtDbContext : DbContext
{

    
    public AtDbContext(DbContextOptions<AtDbContext> options) :base(options)
    {
        Database.EnsureCreated();
    }

    public virtual DbSet<User> Users { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(entity =>
        {
            entity.Property(p => p.Id).ValueGeneratedNever();
            entity.Property(e => e.Birthday).HasColumnType("datetime");
        });
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
        
    }


}
