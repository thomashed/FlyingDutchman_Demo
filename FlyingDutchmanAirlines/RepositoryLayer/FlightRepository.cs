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
        if (int.IsPositive(flightNumber))
        {
            
        }
        
        var flight = await _context.Flights.FirstAsync(
            f => 
                f.FlightNumber == flightNumber && 
                f.Origin == originAirportId && 
                f.Destination == destinationAirportId);

        return flight;
    }
    
    // public int FlightNumber { get; set; }
    // public int Origin { get; set; }
    // public int Destination { get; set; }
    // public ICollection<Booking> Bookings { get; } = new List<Booking>();
    // public Airport DestinationNavigation { get; set; } = null!;
    // public Airport OriginNavigation { get; set; } = null!;
    
}