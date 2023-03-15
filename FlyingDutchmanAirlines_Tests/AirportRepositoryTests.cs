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

        _context.Airports.Add(newAirport);
        await _context.SaveChangesAsync();
        
        _repository = new AirportRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public async Task GetAirportById_Sucess()
    {
        Airport airport = await _repository.GetAirportById(0);
        Assert.IsNotNull(airport);
        Assert.AreEqual(0, airport.AirportId);
        Assert.AreEqual("Nuuk", airport.City);
        Assert.AreEqual(" GOH", airport.Iata);
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
    
}