using ClientMaster.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

builder.Services.AddDbContext<ClientBaseContext>(options => options
    .UseSqlServer(builder.Configuration.GetConnectionString("ClientBaseConnection")));

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    // Reset database upon launch
    var dbContext = scope.ServiceProvider.GetRequiredService<ClientBaseContext>();
    dbContext.Database.EnsureDeleted();
    dbContext.Database.EnsureCreated();

    // Populate database
    if(!dbContext.Clients.Any())
    {
        dbContext.Clients.AddRange(
            new Client
            {
                FirstName = "John",
                LastName = "Doe",
                DateOfBirth = new DateTime(1985, 5, 10),
                StreetAddressLine1 = "123 Main St",
                City = "Anytown",
                State = "California",
                Zip = "12345"
            },
            new Client
            {
                FirstName = "Jane",
                LastName = "Smith",
                DateOfBirth = new DateTime(1990, 8, 20),
                StreetAddressLine1 = "456 Elm St",
                City = "Sometown",
                State = "New York",
                Zip = "54321"
            },
            new Client
            {
                FirstName = "Alice",
                LastName = "Johnson",
                DateOfBirth = new DateTime(1978, 3, 15),
                StreetAddressLine1 = "789 Oak St",
                City = "Anothertown",
                State = "Texas",
                Zip = "67890"
            },
            new Client
            {
                FirstName = "Bob",
                LastName = "Brown",
                DateOfBirth = new DateTime(1995, 10, 5),
                StreetAddressLine1 = "101 Pine St",
                City = "Newville",
                State = "Florida",
                Zip = "13579"
            },
            new Client
            {
                FirstName = "Emily",
                LastName = "Wilson",
                DateOfBirth = new DateTime(1980, 12, 25),
                StreetAddressLine1 = "246 Cedar St",
                City = "Othertown",
                State = "Illinois",
                Zip = "97531"
            }
        );

        dbContext.PhoneNumbers.AddRange(
            new PhoneNumber { Number = "1234567890", ClientID = 1 },
            new PhoneNumber { Number = "1234567891", ClientID = 1 },
            new PhoneNumber { Number = "1234567892", ClientID = 2 },
            new PhoneNumber { Number = "1234567893", ClientID = 3 }
        );

        dbContext.EmailAddresses.AddRange(
            new EmailAddress { Email = "email1@email.com", ClientID = 1 },
            new EmailAddress { Email = "email2@email.com", ClientID = 2 },
            new EmailAddress { Email = "email3@email.com", ClientID = 2 },
            new EmailAddress { Email = "email4@email.com", ClientID = 3 }
        );

        dbContext.SaveChanges();
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
