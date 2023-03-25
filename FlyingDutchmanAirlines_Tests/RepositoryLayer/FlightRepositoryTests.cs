using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.AspNetCore.Http;
using System.Linq;
using FlyingDutchmanAirlines_Tests.RepositoryLayer.Stubs;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class FlightRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;
    private FlightRepository _repository;

    [TestInitialize]
    public async Task TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman").Options;
        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

        Flight flight1 = new Flight()
        {
            FlightNumber = 1,
            Origin = 1,
            Destination = 2
        };

        Flight flight2 = new Flight()
        {
            FlightNumber = 10,
            Origin = 3,
            Destination = 4
        };

        _context.Flights.Add(flight1);
        _context.Flights.Add(flight2);
        await _context.SaveChangesAsync();

        _repository = new FlightRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public async Task GetFlightByFlightNumber_Success()
    {
        Flight flight = await _repository.GetFlightByFlightNumber(1);
        Flight dbFlight = await _context.Flights.FirstAsync(f => f.FlightNumber == 1);

        Assert.IsNotNull(dbFlight);
        Assert.AreEqual(flight.FlightNumber, dbFlight.FlightNumber);
        Assert.AreEqual(flight.Origin, dbFlight.Origin);
        Assert.AreEqual(flight.Destination, dbFlight.Destination);
    }

    [TestMethod]
    [DataRow(-1)]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlightByFlightNumber_Failure_InvalidFlightNumber(int flightNumber)
    {
        await _repository.GetFlightByFlightNumber(flightNumber);
    }

    [TestMethod]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlightByFlightNumber_Failure_DatabaseException()
    {
        int invalidArgs = 33;
        await _repository.GetFlightByFlightNumber(invalidArgs);
    }

    [TestMethod]
    public void GetFlights_Success_CorrectNumberOfFlights()
    {
        DbSet<Flight> flightsFromDb = _context.Flights;
        IEnumerable<Flight> flightsFromRepository = _repository.GetFlights();
        
        Assert.IsNotNull(flightsFromDb);
        Assert.IsNotNull(flightsFromRepository);
        Assert.AreEqual(flightsFromDb.Count(), flightsFromRepository.Count());
    }
    
    [TestMethod]
    public void GetFlights_Success_CorrectSequenceOfFlights()
    {
        IEnumerable<Flight> flightsFromDb = _context.Flights;
        IEnumerable<Flight> flightsFromRepository = _repository.GetFlights();
        bool areEqual = flightsFromDb.SequenceEqual(flightsFromRepository);
        
        Assert.IsNotNull(flightsFromDb);
        Assert.IsNotNull(flightsFromRepository);
        Assert.IsTrue(areEqual);
    }
    
    [TestMethod]
    public void GetFlights_Failure_WrongSequenceOfFlights()
    {
        IEnumerable<Flight> flightsFromDb = _context.Flights;
        IEnumerable<Flight> flightsFromRepository = _repository.GetFlights();
        IEnumerable<Flight> miscellaneousFlights = 
            new Flight[1] { new Flight()
            {
                FlightNumber = 42,
                Destination = 42,
                Origin = 42
            }};
        IEnumerable<Flight> flightsInWrongSequence = flightsFromRepository.Concat(miscellaneousFlights);
        
        bool areEqual = flightsFromDb.SequenceEqual(flightsInWrongSequence);
        
        Assert.IsNotNull(flightsFromDb);
        Assert.IsNotNull(flightsFromRepository);
        Assert.IsFalse(areEqual);
    }
    
    // TODO: expect some exception and that we can handle it, mock out context for this?
}