using ClientMaster.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace ClientMaster.Pages;

[BindProperties]
public class EditClientModel : PageModel
{
    private readonly ClientBaseContext ClientBaseContext;
    public EditClientModel(ClientBaseContext CBC)
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

    public async Task<IActionResult> OnGetAsync(int ClientID)
    {
        MessageColor = "Green";
        Message = "Edit any information and click Update Client.";

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

    public async Task<IActionResult> OnPostUpdateClientAsync()
    {

        var selectedNumbers = Request.Form["selectedNumbers"].ToList();
        var selectedEmails = Request.Form["selectedEmails"].ToList();

        try
        {
            foreach (var id in selectedNumbers)
            {
                PhoneNumber number = ClientBaseContext.PhoneNumbers.Find(Int32.Parse(id));
                if (number != null)
                {
                    ClientBaseContext.PhoneNumbers.Remove(number);
                }
            }

            foreach (var id in selectedEmails)
            {
                EmailAddress email = ClientBaseContext.EmailAddresses.Find(Int32.Parse(id));
                if (email != null)
                {
                    ClientBaseContext.EmailAddresses.Remove(email);
                }

            }

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

            ClientBaseContext.Clients.Update(Client);
            await ClientBaseContext.SaveChangesAsync();

            TempData["strMessageColor"] = "Green";
            TempData["strMessage"] = Client.FirstName + " " + Client.LastName + " was successfully updated.";
        }
        catch (DbUpdateException dbUpdateException)
        {
            TempData["strMessageColor"] = "Red";
            TempData["strMessage"] = Client.FirstName + " " + Client.LastName + " was NOT updated: " + dbUpdateException.InnerException.Message;
        }
        return Redirect("Clients");
    }

}
