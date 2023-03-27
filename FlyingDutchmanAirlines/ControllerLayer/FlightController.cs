using System.Net;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer;

public class FlightController : Controller
{
    private FlightService _flightService;

    public FlightController(FlightService flightService)
    {
        _flightService = flightService;
    }

    public IActionResult GetFlights()
    {
        return StatusCode((int)HttpStatusCode.OK, "Hello World!");
    }
}