using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.IdentityModel.Tokens;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class BookingService
{
    private readonly BookingRepository _bookingRepository;
    private readonly CustomerRepository _customerRepository;
    private readonly FlightRepository _flightRepository;

    public BookingService(BookingRepository bookingRepository)
    {
        _bookingRepository = bookingRepository;
    }
    
    public BookingService(BookingRepository bookingRepository, CustomerRepository customerRepository,
        FlightRepository flightRepository)
    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _flightRepository = flightRepository;
    }

    public async Task<(bool result, Exception exception)> CreateBooking(string name, int flightNumber)
    {
        if (!flightNumber.IsPositive() || name.IsNullOrEmpty())
        {
            return (false, new ArgumentException());
        }
        
        try
        {
            Customer customer;
            try
            {
                customer = await _customerRepository.GetCustomerByName(name);
            }
            catch (CustomerNotFoundException e)
            {
                await _customerRepository.CreateCustomer(name);
                return await CreateBooking(name, flightNumber);
            }

            await _bookingRepository.CreateBooking(customer.CustomerId, flightNumber);
            return (true, null);
        }
        catch (Exception e)
        {
            return (false, e);
        }
    }
}