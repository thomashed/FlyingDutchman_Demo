using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
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
        if (!flightNumber.IsPositive())
        {
            Console.WriteLine($"Couldn't find requested flight! " +
                              $"flightNumber: {flightNumber}");
            throw new FlightNotFoundException();
        }
        
        if (!originAirportId.IsPositive() || !destinationAirportId.IsPositive())
        {
            Console.WriteLine($"ArgumentException in GetFlightByFlightNumber! " +
                              $"GetFlightByFlightNumber: {flightNumber}, " +
                              $"originAirportId: {originAirportId}, " +
                              $"destinationAirportId: {destinationAirportId}");
            throw new ArgumentException("Invalid arguments provided");
        }
        
        return await _context.Flights.FirstOrDefaultAsync(f => f.FlightNumber == flightNumber) 
               ?? throw new FlightNotFoundException();
    }

}