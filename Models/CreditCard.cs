using Seido.Utilities.SeedGenerator;

namespace Models;

public class CreditCard : ICreditCard
{
    public virtual CardIssuer Issuer { get; set; }
    public virtual string IssuerString
    {
        get => Issuer.ToString();
        set => Issuer = Enum.Parse<CardIssuer>(value);
    }
    public virtual String CardNumber { get; set; }
    public virtual string ExpiryMonth { get; set; }
    public virtual string ExpiryYear { get; set; }

}
