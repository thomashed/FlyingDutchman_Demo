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
            Airport origin;
            Airport destination;

            try
            {
                origin = await _airportRepository.GetAirportById(flight.Origin);
                destination = await _airportRepository.GetAirportById(flight.Destination);
            }
            catch (FlightNotFoundException) // TODO: should be AirportNotFoundException?
            {
                throw new FlightNotFoundException();
            }
            catch (Exception e)
            {
                throw new ArgumentException();
            }
            
            yield return new FlightView(
                flight.FlightNumber.ToString(),
                (origin.City, origin.Iata),
                (destination.City, destination.Iata)
                );
        }
    }

}