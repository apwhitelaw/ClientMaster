using ClientMaster.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ClientMaster.Pages;

[BindProperties]
public class DeleteClientModel : PageModel
{
    private readonly ClientBaseContext ClientBaseContext;
    public DeleteClientModel(ClientBaseContext CBC)
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
        Message = "Edit any information and click Update Client.";

        Client = await ClientBaseContext.Clients.FindAsync(ClientID);

        if (Client != null)
        {
            return Page();
        }
        else
        {
            TempData["strMessage"] = "Client not found.";
            TempData["strMessageColor"] = "Red";

            return Redirect("Clients");
        }
    }

    public async Task<IActionResult> OnPostDeleteClientAsync(string firstName, string lastName)
    {
        try
        {
            ClientBaseContext.Clients.Remove(Client);
            await ClientBaseContext.SaveChangesAsync();

            TempData["strMessageColor"] = "Green";
            TempData["strMessage"] = firstName + " " + lastName + " was successfully removed.";
        }
        catch (DbUpdateException dbUpdateException)
        {
            SqlException dbSqlException = dbUpdateException.InnerException as SqlException;
            if (dbSqlException.Number == 547)
            {
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = firstName + " " + lastName + " was NOT removed because it refers another client.";
            }
            else
            {
                TempData["strMessageColor"] = "Red";
                TempData["strMessage"] = firstName + " " + lastName + " was NOT removed: " + dbUpdateException.InnerException.Message;
            }
        }
        return Redirect("Clients");
    }
}
