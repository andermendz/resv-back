using Microsoft.EntityFrameworkCore;
using SpaceReservation.Domain.Entities;

namespace SpaceReservation.Infrastructure.Persistence;

public class ApplicationDbContext : DbContext
{
    public DbSet<Space> Spaces { get; set; } = null!;
    public DbSet<Reservation> Reservations { get; set; } = null!;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Space>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(100);
            entity.Property(e => e.Description).HasMaxLength(500);
        });

        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Cedula).IsRequired().HasMaxLength(20);
            entity.Property(e => e.StartTime).IsRequired();
            entity.Property(e => e.EndTime).IsRequired();

            entity.HasOne(e => e.Space)
                .WithMany()
                .HasForeignKey(e => e.SpaceId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Seed some initial spaces
        modelBuilder.Entity<Space>().HasData(
            new Space("Conference Room A", "Main conference room with projector") { Id = 1 },
            new Space("Meeting Room B", "Small meeting room for up to 6 people") { Id = 2 },
            new Space("Auditorium", "Large presentation space") { Id = 3 }
        );
    }
} 