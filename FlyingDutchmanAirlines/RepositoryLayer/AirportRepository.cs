using System.Reflection;
using System.Runtime.CompilerServices;
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

    [MethodImpl(MethodImplOptions.NoInlining)]
    public AirportRepository()
    {
        if (Assembly.GetCallingAssembly().FullName == Assembly.GetExecutingAssembly().FullName)
        {
            throw new Exception("This constructor should only be used for testing");
        }
    }

    public virtual async Task<Airport> GetAirportById(int airportID)
    {
        if (!airportID.IsPositive())
        {
            Console.WriteLine($"ArgumentException in GetAirportById! AirportId: " +
                              $"{airportID}");
            throw new ArgumentException("Invalid arguments provided");
        }
        
        return await _context.Airports.FirstOrDefaultAsync(a => a.AirportId == airportID) 
               ?? throw new AirportNotFoundException();
    }
}