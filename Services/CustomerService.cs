using Microsoft.Extensions.Logging;
using Configuration;
using Models;
using Seido.Utilities.SeedGenerator;

namespace Services;

public class CustomerService : ICustomerService
{

    public List<ICustomer> GetCustomers(int nrItems)
    {
        var customers = new List<ICustomer>();
        var seeder = new SeedGenerator(); // Add this line to initialize seeder
        for (int i = 0; i < nrItems; i++)
        {
            var customer = new Customer
            {
                FirstName = seeder.FirstName,
                LastName = seeder.LastName,
                CreditCard = new CreditCard
                {
                    Issuer = (CardIssuer)seeder.Next(0, 4),
                    CardNumber = $"{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}-{seeder.Next(2222, 9999)}",
                    ExpiryYear = $"{seeder.Next(25, 32)}",
                    ExpiryMonth = $"{seeder.Next(01, 13):D2}"
                }
            };
            customers.Add(customer);
        }
        return customers;
    }
}