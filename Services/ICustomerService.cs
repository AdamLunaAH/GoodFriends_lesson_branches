using Models;

namespace Services;

public interface ICustomerService
{
    List<ICustomer> GetCustomers(int nrItems);

}