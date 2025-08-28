using System.Globalization;
using Seido.Utilities.SeedGenerator;

namespace Models;

public class CreditCard : ICreditCard
{
    public Guid CreditCardId { get; set; }
    public virtual CardIssuer Issuer { get; set; }
    // public virtual string IssuerString
    // {
    //     get => Issuer.ToString();
    //     set => Issuer = Enum.Parse<CardIssuer>(value);
    // }
    public virtual String CardNumber { get; set; }
    public virtual string ExpiryYear { get; set; }
    public virtual string ExpiryMonth { get; set; }



    #region Seeder

    public bool Seeded { get; set; } = false;
    public virtual CreditCard seed(SeedGenerator seeder)
    {
        Seeded = true;
        CreditCardId = Guid.NewGuid();
        Issuer = seeder.FromEnum<CardIssuer>();
        CardNumber = $"{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}";
        ExpiryMonth = $"{seeder.Next(25, 32)}";
        ExpiryYear = $"{seeder.Next(01, 13):D2}";
        return this;
    }
    #endregion
}
