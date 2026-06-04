using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace Event_Ease_2026_Ntsika_Nkonki.Models
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Venue> Venues { get; set; }
        public DbSet<Event> Events { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<EventType> EventTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Event>()
    .HasOne(e => e.Venue)
    .WithMany(v => v.Events)
    .HasForeignKey(e => e.VenueId)
    .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Event)
                .WithMany(e => e.Bookings)
                .HasForeignKey(b => b.EventId)
                .OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<Booking>()
                .HasOne(b => b.Venue)
                .WithMany(v => v.Bookings)
                .HasForeignKey(b => b.VenueId)
                .OnDelete(DeleteBehavior.NoAction);

            // Event → EventType 
            modelBuilder.Entity<Event>()
                .HasOne(e => e.EventType)
                .WithMany(et => et.Events)
                .HasForeignKey(e => e.EventTypeId)
                .OnDelete(DeleteBehavior.NoAction);

            // Seed EventTypes 
            modelBuilder.Entity<EventType>().HasData(
                new EventType { EventTypeId = 1, Name = "Concert" },
                new EventType { EventTypeId = 2, Name = "Workshop" },
                new EventType { EventTypeId = 3, Name = "Conference" },
                new EventType { EventTypeId = 4, Name = "Seminar" }
                );
        }
    }
}
