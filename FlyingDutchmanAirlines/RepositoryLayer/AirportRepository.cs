using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class AirportRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public AirportRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }


    public async Task<Airport> GetAirportById(int airportID)
    {
        if (!int.IsPositive(airportID))
        {
            Console.WriteLine($"ArgumentException in GetAirportById! AirportId: " +
                              $"{airportID}");
            throw new ArgumentException("Invalid arguments provided");
        }
        
        return await _context.Airports.FirstOrDefaultAsync(a => a.AirportId == airportID) 
               ?? throw new AirportNotFoundException();
    }
}