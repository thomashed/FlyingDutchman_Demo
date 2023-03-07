using System.Linq;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class CustomerRepository
{
    public bool CreateCustomer(string name)
    {
        if (IsInvalidCustomerName(name))
        {
            return false;
        }

        Customer customer = new Customer(name);

        using (FlyingDutchmanAirlinesContext context = new FlyingDutchmanAirlinesContext())
        {
            context.Customers.Add(customer);
            context.SaveChangesAsync();
        }
        
        return true;
    }

    private bool IsInvalidCustomerName(string name)
    {
        char[] invalidChars = {'!', '@', '#', '$', '%', '&', '*'};
        return string.IsNullOrEmpty(name) || name.Any(x => invalidChars.Contains(x));
    }
    
}