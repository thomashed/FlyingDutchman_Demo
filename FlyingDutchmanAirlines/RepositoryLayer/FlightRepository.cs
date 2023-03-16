using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class FlightRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public FlightRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }

    public async Task<Flight> GetFlightByFlightNumber(int flightNumber, int originAirportId, int destinationAirportId)
    {
        if (!originAirportId.IsPositive() || !destinationAirportId.IsPositive())
        {
            Console.WriteLine($"ArgumentException in GetFlightByFlightNumber! " +
                              $"GetFlightByFlightNumber: {{airportID}}, " +
                              $"originAirportId: {originAirportId}, " +
                              $"destinationAirportId: {destinationAirportId}");
            throw new ArgumentException("Invalid arguments provided");
        }
        
        return new Flight();
    }
    
    // public int FlightNumber { get; set; }
    // public int Origin { get; set; }
    // public int Destination { get; set; }
    // public ICollection<Booking> Bookings { get; } = new List<Booking>();
    // public Airport DestinationNavigation { get; set; } = null!;
    // public Airport OriginNavigation { get; set; } = null!;
    
}