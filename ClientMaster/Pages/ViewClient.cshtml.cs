using ClientMaster.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClientMaster.Pages;

[BindProperties]
public class ViewClientModel : PageModel
{
    private readonly ClientBaseContext ClientBaseContext;
    public ViewClientModel(ClientBaseContext CBC)
    {
        ClientBaseContext = CBC;
    }

    public string Message;
    public string MessageColor;

    public SelectList StateSelectList;
    public SelectList ReferrerSelectList;
    public Client Client { get; set; }

    public async Task<IActionResult> OnGetAsync(int ClientID)
    {
        MessageColor = "Green";
        Message = "";

        Client = await ClientBaseContext.Clients.FindAsync(ClientID);

        if (Client != null)
        {
            await ClientBaseContext.Entry(Client)
                .Reference(c => c.ReferringClient)
                .LoadAsync();

            await ClientBaseContext.Entry(Client)
                .Collection(c => c.PhoneNumbers)
                .LoadAsync();

            await ClientBaseContext.Entry(Client)
                .Collection(c => c.EmailAddresses)
                .LoadAsync();

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

            var ClientList = ClientBaseContext.Clients
                .Where(c => c.ClientID != Client.ClientID)
                .Select(c => new { c.ClientID, Name = c.FirstName + " " + c.LastName }).ToList();
            ReferrerSelectList = new SelectList(ClientList, "ClientID", "Name");

            return Page();
        }
        else
        {
            TempData["strMessage"] = "Client not found.";
            TempData["strMessageColor"] = "Red";

            return Redirect("Clients");
        }
    }
}
