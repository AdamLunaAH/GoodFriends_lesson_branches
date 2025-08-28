using Configuration;
using Seido.Utilities.SeedGenerator;
namespace Models;

public class Customer : ICustomer
{
    public virtual Guid CustomerId { get; set; }
    public virtual string FirstName { get; set; }
    public virtual string LastName { get; set; }
    public virtual ICreditCard CreditCard { get; set; }
    public string CreditCardEncrypted { get; set; }


    #region Seeder
    public bool Seeded { get; set; } = false;

    public virtual Customer Seed(SeedGenerator seeder)
    {
        Seeded = true;
        CustomerId = Guid.NewGuid();

        FirstName = seeder.FirstName;
        LastName = seeder.LastName;

        return this;
    }
    #endregion
}