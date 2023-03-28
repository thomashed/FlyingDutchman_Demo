using System.Net;
using FlyingDutchmanAirlines.ControllerLayer;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer;

[TestClass]
public class FlightControllerTests
{
    private Mock<FlightService> _flightService;
    private List<FlightView> mockReturn;

    [TestInitialize]
    public async Task TestInitialize()
    {
        _flightService = new Mock<FlightService>();

        mockReturn = new List<FlightView>()
        {
            new FlightView("1932", ("Groningen", "GRQ"), ("Phoenix", "PHX")), 
            new FlightView("841",("New York City", "JFK"), ("London", "LHR")) 
        };
        
        _flightService.Setup(service => 
            service.GetFlights()).Returns(FlightViewAsyncGenerator(mockReturn));
    }

    [TestMethod]
    public async Task GetFlights_Success()
    {
        FlightController controller = new FlightController(_flightService.Object);
        ObjectResult response = await controller.GetFlights() as ObjectResult;
        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
        
        Queue<FlightView> content = response.Value as Queue<FlightView>;
        Assert.IsNotNull(content);

        Assert.IsTrue(mockReturn.All(flight => content.Contains(flight)));
    }

    private async IAsyncEnumerable<FlightView> FlightViewAsyncGenerator(IEnumerable<FlightView> views)
    {
        foreach (FlightView view in views)
        {
            yield return view;
        }
    }

}