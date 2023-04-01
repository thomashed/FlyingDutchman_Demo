using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines.ControllerLayer;

[Route("{controller}")]
public class BookingController : Controller
{

    [HttpPost]
    public async Task<IActionResult> CreateBooking([FromBody] BookingData body)
    {

        throw new NotImplementedException();
    }
    
}