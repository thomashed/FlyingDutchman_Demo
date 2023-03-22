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

        Flight flight = new Flight()
        {
            FlightNumber = 1,
            Origin = 1,
            Destination = 2
        };

        _context.Flights.Add(flight);
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
    
    // TODO: check if we need these tests
    // [TestMethod]
    // [DataRow(1,-1,0)]
    // [ExpectedException(typeof(ArgumentException))]
    // public async Task GetFlightByFlightNumber_Failure_InvalidOriginAirport(int flightNumber, int originAirportId, int destinationAirportId)
    // {
    //     await _repository.GetFlightByFlightNumber(flightNumber, originAirportId, destinationAirportId);
    // }
    //
    // [TestMethod]
    // [DataRow(1,1,-1)]
    // [ExpectedException(typeof(ArgumentException))]
    // public async Task GetFlightByFlightNumber_Failure_InvalidDestinationAirport(int flightNumber, int originAirportId, int destinationAirportId)
    // {
    //     await _repository.GetFlightByFlightNumber(flightNumber, originAirportId, destinationAirportId);
    // }

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
    
}