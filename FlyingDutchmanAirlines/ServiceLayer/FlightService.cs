using System.Runtime.ExceptionServices;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.IdentityModel.Tokens;

namespace FlyingDutchmanAirlines.ServiceLayer;

public class FlightService
{
    
    private readonly FlightRepository _flightRepository;
    private readonly AirportRepository _airportRepository;

    public FlightService(FlightRepository flightRepository, AirportRepository airportRepository)
    {
        _flightRepository = flightRepository;
        _airportRepository = airportRepository;
    }

    public async IAsyncEnumerable<FlightView> GetFlights()
    {
        Queue<Flight> flights = new Queue<Flight>(_flightRepository.GetFlights());

        foreach (Flight flight in flights)
        {
            Airport origin = await _airportRepository.GetAirportById(flight.Origin);
            Airport destination = await _airportRepository.GetAirportById(flight.Destination);
            
            yield return new FlightView(
                flight.FlightNumber.ToString(),
                (origin.City, origin.Iata),
                (destination.City, destination.Iata)
                );
        }
    }

}