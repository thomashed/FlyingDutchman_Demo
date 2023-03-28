using System.Net;
using FlyingDutchmanAirlines.ControllerLayer;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
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
    }

    [TestMethod]
    public async Task GetFlights_Success()
    {
        _flightService.Setup(service => 
            service.GetFlights()).Returns(FlightViewAsyncGenerator(mockReturn));
        
        FlightController controller = new FlightController(_flightService.Object);
        ObjectResult response = await controller.GetFlights() as ObjectResult;
        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);
        
        Queue<FlightView> content = response.Value as Queue<FlightView>;
        Assert.IsNotNull(content);

        Assert.IsTrue(mockReturn.All(flight => content.Contains(flight)));
    }

    [TestMethod]
    public async Task GetFlights_Failure_FlightNotFoundException_404()
    {
        _flightService.Setup(service => 
            service.GetFlights()).Throws(new FlightNotFoundException());

        FlightController controller = new FlightController(_flightService.Object);
        ObjectResult response = await controller.GetFlights() as ObjectResult;
        
        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
        Assert.AreEqual("No flights were found in the database", response.Value);
    }

    [TestMethod]
    public async Task GetFlights_Failure_ArgumentException_500()
    {
        _flightService.Setup(service => 
            service.GetFlights()).Throws(new ArgumentException());

        FlightController controller = new FlightController(_flightService.Object);
        ObjectResult response = await controller.GetFlights() as ObjectResult;
        
        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.AreEqual("Internal server error", response.Value);
    }

    private async IAsyncEnumerable<FlightView> FlightViewAsyncGenerator(IEnumerable<FlightView> views)
    {
        foreach (FlightView view in views)
        {
            yield return view;
        }
    }

}