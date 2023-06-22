using API.Models;
using Microsoft.EntityFrameworkCore;

namespace API.Data;

public class BookingDbContext : DbContext
{
    public BookingDbContext(DbContextOptions<BookingDbContext> options) : base(options)
    {

    }

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
        modelBuilder.Entity<Booking>()
                    .HasIndex(b => new {
                        b.RoomGuid,
                        b.EmployeeGuid
                    }).IsUnique();

        modelBuilder.Entity<Employee>()
                    .HasIndex(e => new {
                        e.Nik,
                        e.Email,
                        e.PhoneNumber
                    }).IsUnique();

        modelBuilder.Entity<AccountRole>()
                    .HasIndex(a => new {
                        a.AccountGuid,
                        a.RoleGuid
                    }).IsUnique();


        modelBuilder.Entity<Education>()
                    .HasIndex(ed => new {
                        ed.UniversityGuid
                    }).IsUnique();

        // Account - Account Role (One to Many)
        modelBuilder.Entity<Account>()
                    .HasMany(account => account.AccountRoles)
                    .WithOne(AccountRoles => AccountRoles.Account)
                    .HasForeignKey(account => account.AccountGuid);
                    // .OnDelete(DeleteBehavior.Cascade);

        // University - Education (One to Many)
        modelBuilder.Entity<University>()
                    .HasMany(university => university.Educations)
                    .WithOne(education => education.University)
                    .HasForeignKey(education => education.UniversityGuid);

        // Role - AccountRole (One to Many)


        // Account - AccountRole (One to Many)


    }
}