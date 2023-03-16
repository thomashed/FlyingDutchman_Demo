using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Runtime.ExceptionServices;
using FlyingDutchmanAirlines_Tests.RepositoryLayer.Stubs;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class AirportRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;
    private AirportRepository _repository;

    [TestInitialize]
    public async Task TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman").Options;
        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

        Airport newAirport = new Airport()
        {
            AirportId = 0,
            City = "Nuuk",
            Iata = " GOH"
        };

        SortedList<string, Airport> airports = new SortedList<string, Airport>()
        {
            {
                "GOH", 
                new Airport()
                {
                    AirportId = 0,
                    City = "Nuuk",
                    Iata = " GOH"
                }
            },
            {
                "PHX",
                new Airport
                {
                    AirportId = 1,
                    City = "Phoenix",
                    Iata = "PHX"
                }
            },
            {
                "DDH",
                new Airport
                {
                    AirportId = 2,
                    City = "Bennington",
                    Iata = "DDH"
                }
            },
            {
                "RDU",
                new Airport
                {
                    AirportId = 3,
                    City = "Raleigh-Durham",
                    Iata = "RDU"
                }
            }
        }; 

        _context.Airports.AddRange(airports.Values);
        await _context.SaveChangesAsync();
        
        _repository = new AirportRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    [DataRow(0)]
    [DataRow(1)]
    [DataRow(2)]
    [DataRow(3)]
    public async Task GetAirportById_Success(int airportID)
    {
        Airport airport = await _repository.GetAirportById(airportID);
        Airport airportDB = _context.Airports.First(airport => airport.AirportId == airportID);

        Assert.AreEqual(airportDB.AirportId, airport.AirportId);
        Assert.AreEqual(airportDB.Iata, airport.Iata);
        Assert.AreEqual(airportDB.City, airport.City);
    }

    [TestMethod]
    [ExpectedException(typeof(ArgumentException))]
    public async Task GetAirportByID_Failure_InvalidInputs()
    {
        int invalidId = -1;
        string expectedErrorMessage = $"ArgumentException in GetAirportById! AirportId: {invalidId}";
        StringWriter outputStream = new StringWriter();
        
        try
        {
            Console.SetOut(outputStream);
            await _repository.GetAirportById(invalidId);

        }
        catch (ArgumentException e)
        {
            Assert.IsTrue(outputStream.ToString().Contains(expectedErrorMessage));
            throw;
        }
        finally
        {
            outputStream.Dispose(); // we dispose the old-fashioned way in order to have outputStream within scope
        }
    }

    [TestMethod]
    [ExpectedException(typeof(AirportNotFoundException))]
    public async Task GetAirportByID_Failure_DatabaseException()
    {
        await _repository.GetAirportById(10);
    }
    
}