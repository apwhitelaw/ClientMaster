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

    public Client Client { get; set; }

    public async Task<IActionResult> OnGetAsync(int ClientID)
    {
        MessageColor = "Green";
        Message = "";

        Client = await ClientBaseContext.Clients.FindAsync(ClientID);

        if (Client != null)
        {
            // Load other properties related to Client from database
            await ClientBaseContext.Entry(Client)
                .Reference(c => c.ReferringClient)
                .LoadAsync();

            await ClientBaseContext.Entry(Client)
                .Collection(c => c.PhoneNumbers)
                .LoadAsync();

            await ClientBaseContext.Entry(Client)
                .Collection(c => c.EmailAddresses)
                .LoadAsync();

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
