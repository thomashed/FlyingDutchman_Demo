using FlyingDutchmanAirlines.DatabaseLayer;

namespace FlyingDutchmanAirlines.RepositoryLayer;

public class FlightRepository
{
    private readonly FlyingDutchmanAirlinesContext _context;

    public FlightRepository(FlyingDutchmanAirlinesContext context)
    {
        _context = context;
    }
    
    
}