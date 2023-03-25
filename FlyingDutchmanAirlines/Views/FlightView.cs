using FlyingDutchmanAirlines.DatabaseLayer.Models;
using System.Runtime.ExceptionServices;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.IdentityModel.Tokens;

namespace FlyingDutchmanAirlines.Views;

public class FlightView
{
    public string FlightNumber { get; private set; }
    public AirportInfo Origin { get; private set; }
    public AirportInfo Destination { get; private set; }

    public FlightView(string flightNumber, 
        (string city, string code) origin, 
        (string city, string code) destination)
    {
        FlightNumber = flightNumber.IsNullOrEmpty() ? "no flight-number found" : flightNumber;
        Origin = new AirportInfo(origin);
        Destination = new AirportInfo(destination);
    }
}

public struct AirportInfo
{
    public string City { get; set; }
    public string Code { get; set; }

    public AirportInfo((string city, string code) airport)
    {
        City = airport.city.IsNullOrEmpty() ? "No city found" : airport.city;
        Code = airport.code.IsNullOrEmpty() ? "No code found" : airport.code;
    }

}