using FlyingDutchmanAirlines.RepositoryLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using FlyingDutchmanAirlines.Views;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer;

[TestClass]
public class FlightServiceTests
{
    private Mock<FlightRepository> _mockFlightRepository;
    private Mock<AirportRepository> _mockAirportRepository;

    private readonly int _flightInDatabaseOriginID = 31;
    private readonly int _flightInDatabaseDestinationID = 92;
    
    [TestInitialize]
    public async Task TestInitialize()
    {
        _mockFlightRepository = new Mock<FlightRepository>();
        _mockAirportRepository = new Mock<AirportRepository>();
        
        Flight flightInDatabase = new Flight { 
            FlightNumber = 148, 
            Origin = _flightInDatabaseOriginID, 
            Destination = _flightInDatabaseDestinationID 
        };

        Queue<Flight> mockReturn = new Queue<Flight>(1);
        mockReturn.Enqueue(flightInDatabase);

        _mockFlightRepository.Setup(
            repository => repository
                .GetFlights()).Returns(mockReturn);
    }
    
    [TestMethod]
    public async Task GetFlights_Success()
    {
        Airport[] airportsInDatabase = new Airport[2]
        {
            new Airport(){
                AirportId = 31, 
                City = "Mexico City", 
                Iata = "MEX" 
            },
            new Airport(){
                AirportId = 92, 
                City = "Ulaanbaataar", 
                Iata = "UBN" 
            }
        };

        _mockAirportRepository.Setup(
                repository => repository.GetAirportById(_flightInDatabaseOriginID))
            .ReturnsAsync(airportsInDatabase[0]);
        _mockAirportRepository.Setup(
                repository => repository.GetAirportById(_flightInDatabaseDestinationID))
            .ReturnsAsync(airportsInDatabase[1]);
        
        FlightService service = new FlightService(
            _mockFlightRepository.Object, 
            _mockAirportRepository.Object);

        await foreach (FlightView flightView in service.GetFlights())
        {
            Assert.IsNotNull(flightView);
            Assert.AreEqual(flightView.FlightNumber, "148");
            Assert.AreEqual(flightView.Origin.City, "Mexico City");
            Assert.AreEqual(flightView.Origin.Code, "MEX");
            Assert.AreEqual(flightView.Destination.City, "Ulaanbaataar");
            Assert.AreEqual(flightView.Destination.Code, "UBN");
        }
    }

    [TestMethod]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlights_Failure_RepositoryException()
    {
        _mockAirportRepository.Setup(repository => repository
                .GetAirportById(_flightInDatabaseOriginID))
            .ThrowsAsync(new FlightNotFoundException());

        FlightService service = new FlightService(
            _mockFlightRepository.Object,
            _mockAirportRepository.Object);

        await foreach (FlightView _ in service.GetFlights())
        {
            ;
        }
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetFlights_Failure_RegularException()
    {
        _mockAirportRepository.Setup(repository => repository
            .GetAirportById(_flightInDatabaseOriginID)).ThrowsAsync(new NullReferenceException());

        FlightService service = new FlightService(
            _mockFlightRepository.Object,
            _mockAirportRepository.Object);

        await foreach (FlightView _ in service.GetFlights())
        {
            ;
        }
    }
}