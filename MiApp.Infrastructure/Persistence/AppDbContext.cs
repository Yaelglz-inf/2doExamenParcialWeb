using Microsoft.EntityFrameworkCore;
using MiApp.Domain.Entities;
using MiApp.Domain.Enums;

namespace MiApp.Infrastructure.Persistence;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<Event> Events => Set<Event>();
    public DbSet<TicketZone> TicketZones => Set<TicketZone>();
    public DbSet<TicketPurchase> TicketPurchases => Set<TicketPurchase>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Configurar User
        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(u => u.Id);
            entity.HasIndex(u => u.Email).IsUnique();
            entity.Property(u => u.Email).IsRequired().HasMaxLength(256);
            entity.Property(u => u.PasswordHash).IsRequired();
            entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
            entity.Property(u => u.Role).HasConversion<string>().IsRequired();
        });

        // Configurar Event
        modelBuilder.Entity<Event>(entity =>
        {
            entity.HasKey(e => e.Id);
            entity.Property(e => e.Name).IsRequired().HasMaxLength(256);
            entity.Property(e => e.Description).IsRequired().HasMaxLength(1000);
            entity.Property(e => e.Location).IsRequired().HasMaxLength(500);
            entity.Property(e => e.Status).HasConversion<string>().IsRequired();
            entity.HasMany(e => e.TicketZones)
                .WithOne(tz => tz.Event)
                .HasForeignKey(tz => tz.EventId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasMany(e => e.TicketPurchases)
                .WithOne(tp => tp.Event)
                .HasForeignKey(tp => tp.EventId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Configurar TicketZone
        modelBuilder.Entity<TicketZone>(entity =>
        {
            entity.HasKey(tz => tz.Id);
            entity.Property(tz => tz.Name).IsRequired().HasMaxLength(100);
            entity.Property(tz => tz.Price).HasPrecision(10, 2);
            entity.HasMany(tz => tz.TicketPurchases)
                .WithOne(tp => tp.TicketZone)
                .HasForeignKey(tp => tp.TicketZoneId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Configurar TicketPurchase
        modelBuilder.Entity<TicketPurchase>(entity =>
        {
            entity.HasKey(tp => tp.Id);
            entity.Property(tp => tp.UnitPrice).HasPrecision(10, 2);
            entity.Property(tp => tp.TotalPrice).HasPrecision(10, 2);
            entity.HasOne(tp => tp.User)
                .WithMany()
                .HasForeignKey(tp => tp.UserId)
                .OnDelete(DeleteBehavior.Restrict);
        });
    }
}
