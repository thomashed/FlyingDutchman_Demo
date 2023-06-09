using System.Data;
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

    [TestMethod]
    public async Task GetFlightByFlightNumber_Success()
    {
        FlightView returnedFlightView = new FlightView(
                "0", ("Lagos", "LOS"),
                ("Marrakesh", "RAK"));
        _flightService.Setup(service =>
            service.GetFlightByFlightNumber(0)).Returns(Task.FromResult(returnedFlightView)); // TODO: try returnsAsync
     
        FlightController flightController = new FlightController(_flightService.Object);
        ObjectResult response = await flightController.GetFlightByFlightNumber(0) as ObjectResult;
        
        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.OK, response.StatusCode);

        FlightView content = response.Value as FlightView;
        Assert.IsNotNull(content);
        Assert.AreEqual(returnedFlightView, content);
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Failure_FlightNotFoundException_404()
    {
        _flightService.Setup(service =>
            service.GetFlightByFlightNumber(42)).Throws(new FlightNotFoundException());
        
        FlightController controller = new FlightController(_flightService.Object);

        ObjectResult response = await controller.GetFlightByFlightNumber(42) as ObjectResult;
        
        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.NotFound, response.StatusCode);
        Assert.AreEqual("No flight were found in the database", response.Value);
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Failure_NullReferenceException_500()
    {
        _flightService.Setup(service => 
            service.GetFlightByFlightNumber(21)).Throws(new NullReferenceException());
        
        FlightController controller = new FlightController(_flightService.Object);

        ObjectResult response = await controller.GetFlightByFlightNumber(21) as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.InternalServerError, response.StatusCode);
        Assert.AreEqual("An internal server error occurred", response.Value);
    }

    [TestMethod]
    [DataRow(1)]
    public async Task GetFlightByFlightNumber_Failure_ArgumentException_400(int flightNumber)
    {
        _flightService.Setup(service =>
            service.GetFlightByFlightNumber(1)).Throws(new ArgumentException());

        FlightController controller = new FlightController(_flightService.Object);

        ObjectResult response = await 
            controller.GetFlightByFlightNumber(flightNumber) as ObjectResult;

        Assert.IsNotNull(response);
        Assert.AreEqual((int)HttpStatusCode.BadRequest, response.StatusCode);
        Assert.AreEqual("Bad request", response.Value);
    }
    
    private async IAsyncEnumerable<FlightView> FlightViewAsyncGenerator(IEnumerable<FlightView> views)
    {
        foreach (FlightView view in views)
        {
            yield return view;
        }
    }
}