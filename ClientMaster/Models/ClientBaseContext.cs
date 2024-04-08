namespace ClientMaster.Models;

using Microsoft.EntityFrameworkCore;

public class ClientBaseContext : DbContext
{
    public DbSet<Client> Clients { set; get; }
    public DbSet<PhoneNumber> PhoneNumbers { set; get; }
    public DbSet<EmailAddress> EmailAddresses { set; get; }

    public ClientBaseContext(DbContextOptions<ClientBaseContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Client>()
            .HasMany(c => c.PhoneNumbers)
            .WithOne(p => p.Client)
            .HasForeignKey(p => p.ClientID);

        modelBuilder.Entity<Client>()
            .HasMany(c => c.EmailAddresses)
            .WithOne(e => e.Client)
            .HasForeignKey(e => e.ClientID);

        modelBuilder.Entity<Client>()
            .Property(c => c.FirstName)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Client>()
            .Property(c => c.LastName)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Client>()
            .Property(c => c.DateOfBirth)
            .IsRequired();

        modelBuilder.Entity<Client>()
            .Property(c => c.StreetAddressLine1)
            .HasMaxLength(100)
            .IsRequired();

        modelBuilder.Entity<Client>()
            .Property(c => c.StreetAddressLine2)
            .HasMaxLength(100);

        modelBuilder.Entity<Client>()
            .Property(c => c.City)
            .HasMaxLength(50)
            .IsRequired();

        modelBuilder.Entity<Client>()
            .Property(c => c.State)
            .HasMaxLength(20)
            .IsRequired();

        modelBuilder.Entity<Client>()
            .Property(c => c.Zip)
            .HasMaxLength(5)
            .IsRequired();


    }

}
