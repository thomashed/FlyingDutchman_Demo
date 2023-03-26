using System;
using System.Collections.Generic;
using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines.DatabaseLayer.Models;

public sealed class Customer
{
    public int CustomerId { get; set; }

    public string Name { get; set; }

    public ICollection<Booking> Bookings { get; }

    public Customer(string name)
    {
        Bookings = new List<Booking>();
        Name = name;
    }

    public static bool operator == (Customer x, Customer y)
    {
        CustomerRepository.CustomerEqualityComparer comparer = new CustomerRepository.CustomerEqualityComparer();
        return comparer.Equals(x,y);
    }

    public static bool operator != (Customer x, Customer y) => !(x == y);
}
