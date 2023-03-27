using System.Net;
using FlyingDutchmanAirlines.ControllerLayer;
using Microsoft.AspNetCore.Mvc;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer;

[TestClass]
public class FlightControllerTests
{
    [TestInitialize]
    public async Task TestInitialize()
    {
        
    }

    [TestMethod]
    public void GetFlights_Success()
    {
        FlightController controller = new FlightController(null);
        ObjectResult response = controller.GetFlights() as ObjectResult;
        
        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
        Assert.AreEqual("Hello World!", response.Value);
    }
    

}