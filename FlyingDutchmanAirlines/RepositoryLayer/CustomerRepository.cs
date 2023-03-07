using System.Linq;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class CustomerRepository
{
    public bool CreateCustomer(string name)
    {
        if (IsInvalidCustomerName(name))
        {
            return false;
        }
        return true;
    }

    private bool IsInvalidCustomerName(string name)
    {
        char[] invalidChars = {'!', '@', '#', '$', '%', '&', '*'};
        return string.IsNullOrEmpty(name) || name.Any(x => invalidChars.Contains(x));
    }
    
}