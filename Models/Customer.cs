namespace Models;

public class Customer : ICustomer
{
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual ICreditCard CreditCard { get; set; }
    public virtual IEncryptedCard EncryptedCardData { get; set; }
}