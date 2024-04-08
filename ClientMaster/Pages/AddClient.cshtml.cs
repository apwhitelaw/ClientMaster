using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientMaster.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ClientMaster.Pages;

[BindProperties]
public class AddClientModel : PageModel
{

    private readonly ClientBaseContext ClientBaseContext;
    public AddClientModel(ClientBaseContext CBC)
    {
        ClientBaseContext = CBC;
    }

    public string Message;
    public string MessageColor;

    public SelectList StateSelectList;
    public SelectList ReferrerSelectList;
    public Client Client { get; set; }

    public string AddNumber { get; set; }
    public string AddEmail { get; set; }

    public void OnGet()
    {
        MessageColor = "Green";
        Message = "Please enter the required information and click Add Client.";

        var states = new List<string>
        {
            "Alabama", "Alaska", "Arizona", "Arkansas", "California",
            "Colorado", "Connecticut", "Delaware", "Florida", "Georgia",
            "Hawaii", "Idaho", "Illinois", "Indiana", "Iowa",
            "Kansas", "Kentucky", "Louisiana", "Maine", "Maryland",
            "Massachusetts", "Michigan", "Minnesota", "Mississippi", "Missouri",
            "Montana", "Nebraska", "Nevada", "New Hampshire", "New Jersey",
            "New Mexico", "New York", "North Carolina", "North Dakota", "Ohio",
            "Oklahoma", "Oregon", "Pennsylvania", "Rhode Island", "South Carolina",
            "South Dakota", "Tennessee", "Texas", "Utah", "Vermont",
            "Virginia", "Washington", "West Virginia", "Wisconsin", "Wyoming"
        };

        StateSelectList = new SelectList(states);

        var ClientList = ClientBaseContext.Clients.Select(c => new { c.ClientID, Name = c.FirstName + " " + c.LastName }).ToList();
        ReferrerSelectList = new SelectList(ClientList, "ClientID", "Name");
    }

    public async Task<IActionResult> OnPostAddClientAsync()
    {
        try
        {

            ClientBaseContext.Clients.Add(Client);
            await ClientBaseContext.SaveChangesAsync();

            if (AddNumber != null)
            {
                PhoneNumber number = new PhoneNumber { ClientID = Client.ClientID, Number = AddNumber };
                ClientBaseContext.PhoneNumbers.Add(number);
            }

            if (AddEmail != null)
            {
                EmailAddress email = new EmailAddress { ClientID = Client.ClientID, Email = AddEmail };
                ClientBaseContext.EmailAddresses.Add(email);
            }

            await ClientBaseContext.SaveChangesAsync();

            TempData["strMessageColor"] = "Green";
            TempData["strMessage"] = Client.FirstName + " " + Client.LastName + " was successfully added.";
        }
        catch (DbUpdateException dbUpdateException)
        {
            TempData["strMessageColor"] = "Red";
            TempData["strMessage"] = Client.FirstName + " " + Client.LastName + " was NOT added: " + dbUpdateException.InnerException.Message;
        }
        return Redirect("Clients");
    }
}
