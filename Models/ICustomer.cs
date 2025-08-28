namespace Models;

public interface ICustomer
{
    public Guid CustomerId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public ICreditCard CreditCard { get; set; }
    // public IEncryptedCard EncryptedCardData { get; set; }

}
