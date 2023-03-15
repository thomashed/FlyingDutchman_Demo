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
public class AirportRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context;
    private AirportRepository _repository;

    [TestInitialize]
    public void TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman").Options;
        _context = new FlyingDutchmanAirlinesContext_Stub(dbContextOptions);

        _repository = new AirportRepository(_context);
        Assert.IsNotNull(_repository);
    }

    [TestMethod]
    public async Task GetAirportById_Sucess()
    {
        Airport airport = await _repository.GetAirportById(0);
        Assert.IsNotNull(airport);
    }
    
}