using ClientMaster.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace ClientMaster.Pages;

public class ClientsModel : PageModel
{

    private readonly ClientBaseContext ClientBaseContext;
    public ClientsModel(ClientBaseContext CBC)
    {
        ClientBaseContext = CBC;
    }

    public string Message;
    public string MessageColor;

    public IList<Client> ClientIList;

    public async Task OnGetAsync()
    {
        if (TempData["strMessage"] == null)
        {
            TempData["strMessage"] = "Select a client.";
            TempData["strMessageColor"] = "Green";
        }
        else
        {
            Message = TempData["strMessage"].ToString();
            MessageColor = TempData["strMessageColor"].ToString();
        }


        ClientIList = await ClientBaseContext.Clients.ToListAsync();
    }
}
