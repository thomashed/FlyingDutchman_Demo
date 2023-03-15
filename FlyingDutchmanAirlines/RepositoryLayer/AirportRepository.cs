using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;

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
        // Steps of retrieving an airport:
        // 1: Arg validation, throw exception if needed 
        // 2: Check dbContext's Airport collection giving id as arg
        // 3: check for db error
        // 4: return the value if found 
        
        return new Airport();
    }
}