namespace ClientMaster.Models;

public class Client
{
    public int ClientID { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public DateTime DateOfBirth { get; set; }
    public string StreetAddressLine1 { get; set; }
    public string? StreetAddressLine2 { get; set; }
    public string City { get; set; }
    public string State { get; set; }
    public string Zip { get; set; }
    public int? ReferringClientID { get; set; }
    public virtual Client ReferringClient { get; set; }
    public virtual ICollection<PhoneNumber> PhoneNumbers { get; set; }
    public virtual ICollection<EmailAddress> EmailAddresses { get; set; }
}
