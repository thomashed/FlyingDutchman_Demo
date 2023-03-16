using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class BookingRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public BookingRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public async Task CreateBooking(int customerID, int flightNumber)
    {
        if ( !customerID.IsPositive() || !flightNumber.IsPositive())
        {
            Console.WriteLine($"ArgumentException in CreateBooking! CustomerId " +
                              $"= {customerID}, flightnumber = {flightNumber}");
            throw new ArgumentException("Invalid arguments provided");
        }

        Booking newBooking = new Booking()
        {
            FlightNumber = flightNumber, 
            CustomerId = customerID
        };

        try
        {
            _context.Bookings.Add(newBooking);
            await _context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine($"Exception during database query: {e.Message}");
            throw new CouldNotAddBookingToDatabaseException();
        }
    }
}