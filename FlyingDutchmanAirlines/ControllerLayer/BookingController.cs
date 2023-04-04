using System.Net;
using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;

namespace FlyingDutchmanAirlines.ControllerLayer;

[Route("{controller}")]
public class BookingController : Controller
{
    private BookingService _bookingService;

    public BookingController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost("{flightNumber}")]
    [ProducesResponseType(StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> CreateBooking([FromBody] BookingData body, int flightNumber)
    {

        if (ModelState.IsValid && flightNumber.IsPositive())
        {
            string name = $"{body.FirstName} {body.LastName}";
            (bool result, Exception exception) = await _bookingService.CreateBooking(name, flightNumber);

            if (result && exception == null)
            {
                return StatusCode((int)HttpStatusCode.Created);
            }

            return StatusCode((int)HttpStatusCode.InternalServerError, exception.Message);
        }

        return StatusCode((int)HttpStatusCode.InternalServerError, 
            ModelState.Root.Errors.First().ErrorMessage);
    }
    
}