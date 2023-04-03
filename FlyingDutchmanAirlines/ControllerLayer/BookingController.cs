using System.Net;
using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer;

[Route("{controller}")]
public class BookingController : Controller
{
    private BookingService _bookingService;

    public BookingController(BookingService bookingService)
    {
        _bookingService = bookingService;
    }

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingData body)
    {

        if (ModelState.IsValid)
        {
            
        }

        return StatusCode((int)HttpStatusCode.InternalServerError, 
            ModelState.Root.Errors.First().ErrorMessage);
    }
    
}