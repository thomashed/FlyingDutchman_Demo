using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer;

public class FlightController : Controller
{
    private FlightService _flightService;

    public FlightController(FlightService flightService)
    {
        _flightService = flightService;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public FlightController()
    {
        if (Assembly.GetCallingAssembly().FullName == Assembly.GetExecutingAssembly().FullName)
        {
            throw new Exception("This constructor should only be used for testing");
        }
    }

    public virtual async Task<IActionResult> GetFlights()
    {
        try
        {
            Queue<FlightView> flights = new Queue<FlightView>();

            await foreach (FlightView flightView in _flightService.GetFlights())
            {
                flights.Enqueue(flightView);
            }
        
            return StatusCode((int)HttpStatusCode.OK, flights);
        }
        catch (FlightNotFoundException e)
        {
            return StatusCode((int)HttpStatusCode.NotFound, "No flights were found in the database");
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, "Internal server error");
        }
    }

    public async Task<IActionResult> GetFlightByFlightNumber(int flightNumber)
    {
        try
        {
            FlightView flightView = await _flightService.GetFlightByFlightNumber(flightNumber);
            return StatusCode((int)HttpStatusCode.OK, flightView);
        }
        catch (FlightNotFoundException)
        {
            return StatusCode((int)HttpStatusCode.NotFound, "Flight not found in the database");
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, "An internal server error occurred");
        }
        
        // return StatusCode((int)HttpStatusCode.OK, "GetFlightByFlightNumber");
        throw new NotImplementedException();
    }
}