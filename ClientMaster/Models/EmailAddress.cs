namespace ClientMaster.Models;

public class EmailAddress
{
    public int EmailAddressID { get; set; }
    public string Email { get; set; }
    public int ClientID { get; set; }
    public virtual Client Client { get; set; }
}
