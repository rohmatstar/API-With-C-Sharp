using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options) { }

    // Table
    public DbSet<Account> Accounts { get; set; }
    public DbSet<AccountRole> AccountRoles { get; set; }
    public DbSet<Education> Educations { get; set; }
    public DbSet<Booking> Bookings { get; set; }
    public DbSet<Employee> Employees { get; set; }
    public DbSet<University> Universities { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Room> Rooms { get; set; }

    // Other Configuration or Fluent API
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Constraints Unique
        modelBuilder.Entity<Employee>()
                    .HasIndex(e => new {
                        e.Nik,
                        e.Email,
                        e.PhoneNumber
                    }).IsUnique();

        // Relation between Education and University (1 to Many)
        modelBuilder.Entity<Education>()
                    .HasOne(e => e.University)
                    .WithMany(u => u.Educations)
                    .HasForeignKey(e => e.UniversityGuid);

        // Relation between Education and Employee (1 to 1)
        modelBuilder.Entity<Education>()
                    .HasOne(e => e.Employee)
                    .WithOne(e => e.Education)
                    .HasForeignKey<Education>(e => e.Guid);

        // Relation between Account and Employee (1 to 1)
        modelBuilder.Entity<Account>()
                    .HasOne(a => a.Employee)
                    .WithOne(e => e.Account)
                    .HasForeignKey<Account>(a => a.Guid);

        // Relation between Account and AccountRole (1 to Many)
        modelBuilder.Entity<AccountRole>()
                    .HasOne(a => a.Account)
                    .WithMany(a => a.AccountRoles)
                    .HasForeignKey(a => a.AccountGuid);

        // Relation between Role and AccountRole (1 to Many)
        modelBuilder.Entity<AccountRole>()
                    .HasOne(a => a.Role)
                    .WithMany(r => r.AccountRoles)
                    .HasForeignKey(a => a.RoleGuid);

        // Relation between Booking and Room (1 to Many)
        modelBuilder.Entity<Booking>()
                    .HasOne(b => b.Room)
                    .WithMany(r => r.Bookings)
                    .HasForeignKey(b => b.RoomGuid);
    }
}