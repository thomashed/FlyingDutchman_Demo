using System.Runtime.ExceptionServices;
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

    public BookingService(BookingRepository bookingRepository) // TODO can we delete this ctor?
    {
        _bookingRepository = bookingRepository;
    }
    
    public BookingService(
        BookingRepository bookingRepository, 
        CustomerRepository customerRepository,
        FlightRepository flightRepository
        )
    {
        _bookingRepository = bookingRepository;
        _customerRepository = customerRepository;
        _flightRepository = flightRepository;
    }

    public async Task<(bool, Exception)> CreateBooking(string name, int flightNumber)
    {
        if (!flightNumber.IsPositive() || name.IsNullOrEmpty())
        {
            return (false, new ArgumentException());
        }
        
        try
        {
            Customer customer = await GetCustomerFromDatabase(name)
                                ?? await AddCustomerToDatabase(name);
            
            if (!await FlightExistsInDatabase(flightNumber))
            {
                return (false, new CouldNotAddBookingToDatabaseException());
            }

            await _bookingRepository.CreateBooking(customer.CustomerId, flightNumber);
            return (true, null);
        }
        catch (Exception e)
        {
            return (false, e);
        }
    }

    private async Task<bool> FlightExistsInDatabase(int flightNumber)
    {
        try
        {
            return await _flightRepository.GetFlightByFlightNumber(flightNumber) != null;
        }
        catch (FlightNotFoundException)
        {
            return false;
        }
    }

    private async Task<Customer?> GetCustomerFromDatabase(string name)
    {
        try
        {
            return await _customerRepository.GetCustomerByName(name);
        }
        catch (CustomerNotFoundException)
        {
            return null;
        }
        catch (Exception e) // if something else was thrown, something went wrong, re-throw
        {
            ExceptionDispatchInfo.Capture(e.InnerException ?? new Exception()).Throw();
            return null;
        }
    }

    private async Task<Customer> AddCustomerToDatabase(string name)
    {
        await _customerRepository.CreateCustomer(name);
        return await _customerRepository.GetCustomerByName(name);
    }
}