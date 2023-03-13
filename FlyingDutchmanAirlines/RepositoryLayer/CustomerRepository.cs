using System.Linq;
using System.Security.Cryptography;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class CustomerRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public CustomerRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateCustomer(string name)
    {
        if (IsInvalidCustomerName(name))
        {
            return false;
        }

        try
        {
            Customer customer = new Customer(name);
            using (_context)
            {
                _context.Customers.Add(customer);
                await _context.SaveChangesAsync();
            }
        }
        catch
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

    public async Task<Customer> GetCustomerByName(string name)
    {
        if (IsInvalidCustomerName(name))
        {
            throw new CustomerNotFoundException();
        }

        return await _context.Customers.FirstOrDefaultAsync(customer => customer.Name == name) 
            ?? throw new CustomerNotFoundException();
    }
   
    internal class CustomerEqualityComparer : EqualityComparer<Customer>
    {
        public override bool Equals(Customer? x, Customer? y)
        {
            return x.Name == y.Name 
                && x.CustomerId == y.CustomerId;
        }

        public override int GetHashCode(Customer obj)
        {
            int randomNumber = RandomNumberGenerator.GetInt32(int.MaxValue / 2);
            return (obj.CustomerId + obj.Name.Length + randomNumber).GetHashCode();
        }
    }
}