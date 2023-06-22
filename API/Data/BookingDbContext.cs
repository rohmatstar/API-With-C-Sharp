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
                    .WithOne(accountRoles => accountRoles.Accounts)
                    .HasForeignKey(accountRoles => accountRoles.AccountGuid);
                    // .OnDelete(DeleteBehavior.Cascade);

        // University - Education (One to Many)
        modelBuilder.Entity<University>()
                    .HasMany(university => university.Educations)
                    .WithOne(education => education.Universities)
                    .HasForeignKey(education => education.UniversityGuid);

        // Role - Account Role (One to Many)
        modelBuilder.Entity<Role>()
                    .HasMany(role => role.AccountRoles)
                    .WithOne(accountRole => accountRole.Roles)
                    .HasForeignKey(accountRole => accountRole.RoleGuid);

        // Employee - Account (One to One)
        modelBuilder.Entity<Employee>()
                    .HasOne(employee => employee.Accounts)
                    .WithOne(account => account.Employee)
                    .HasForeignKey<Account>(account => account.Guid);

        // Employee - Education (One to One)
        modelBuilder.Entity<Employee>()
                    .HasOne(employee => employee.Educations)
                    .WithOne(education => education.Employees)
                    .HasForeignKey<Education>(education => education.Guid);

        // Employee - Booking (One to Many)
        modelBuilder.Entity<Employee>()
                    .HasMany(employee => employee.Booking)
                    .WithOne(booking => booking.Employee)
                    .HasForeignKey(booking => booking.EmployeeGuid);

        // Room - Booking (One to Many)
        modelBuilder.Entity<Room>()
                    .HasMany(room => room.Booking)
                    .WithOne(booking => booking.Room)
                    .HasForeignKey(booking => booking.RoomGuid);

    }
}