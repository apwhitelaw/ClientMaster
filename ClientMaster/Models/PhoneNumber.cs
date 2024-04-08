namespace ClientMaster.Models;

public class PhoneNumber
{
    public int PhoneNumberID { get; set; }
    public string Number { get; set; }
    public int ClientID { get; set; }
    public virtual Client Client { get; set; }
}
