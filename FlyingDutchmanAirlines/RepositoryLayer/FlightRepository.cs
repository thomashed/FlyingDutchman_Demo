using System.Reflection;
using System.Runtime.CompilerServices;
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

    [MethodImpl(MethodImplOptions.NoInlining)]
    public FlightRepository()
    {
        if (Assembly.GetExecutingAssembly().FullName == Assembly.GetCallingAssembly().FullName)
        {
            throw new Exception("This constructor should only be used for testing");
        }
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