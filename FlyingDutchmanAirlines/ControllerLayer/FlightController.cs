using System.Net;
using System.Reflection;
using System.Runtime.CompilerServices;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer;

[Route("{controller}")]
public class FlightController : Controller
{
    private FlightService _flightService;

    public FlightController(FlightService flightService)
    {
        _flightService = flightService;
    }

    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
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

    [HttpGet("{flightNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> GetFlightByFlightNumber(int flightNumber)
    {
        try
        {
            if (!flightNumber.IsPositive())
            {
                throw new ArgumentException();
            }
            
            FlightView flightView = await _flightService.GetFlightByFlightNumber(flightNumber);
            return StatusCode((int)HttpStatusCode.OK, flightView);
        }
        catch (FlightNotFoundException)
        {
            return StatusCode((int)HttpStatusCode.NotFound, "No flight were found in the database");
        }
        catch (ArgumentException)
        {
            return StatusCode((int)HttpStatusCode.BadRequest, "Bad request");
        }
        catch (Exception e)
        {
            return StatusCode((int)HttpStatusCode.InternalServerError, "An internal server error occurred");
        }
    }
}