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
    public void TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman").Options;
        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

        _repository = new FlightRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    [DataRow(1,-1,0)]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetFlightByFlightNumber_Failure_InvalidOriginAirport(int flightNumber, int originAirportId, int destinationAirportId)
    {
        await _repository.GetFlightByFlightNumber(flightNumber, originAirportId, destinationAirportId);
    }

    [TestMethod]
    [DataRow(1,1,-1)]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetFlightByFlightNumber_Failure_InvalidDestinationAirport(int flightNumber, int originAirportId, int destinationAirportId)
    {
        await _repository.GetFlightByFlightNumber(flightNumber, originAirportId, destinationAirportId);
    }

    [TestMethod]
    [DataRow(-1,1,1)]
    [ExpectedException(typeof(FlightNotFoundException))]
    public async Task GetFlightByFlightNumber_Failure_InvalidFlightNumber(int flightNumber, int originAirportId, int destinationAirportId)
    {
        await _repository.GetFlightByFlightNumber(flightNumber, originAirportId, destinationAirportId);
    }

}