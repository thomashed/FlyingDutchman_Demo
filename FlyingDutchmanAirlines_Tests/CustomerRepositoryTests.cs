using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.AspNetCore.Http;
using System.Linq;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class CustomerRepositoryTests
{
    private FlyingDutchmanAirlinesContext _context; // hold our database context so we can use it in our tests
    private CustomerRepository _repository;
    
    [TestInitialize]
    public async Task TestInitialize()
    {
        DbContextOptions<FlyingDutchmanAirlinesContext> dbContextOptions =
            new DbContextOptionsBuilder<FlyingDutchmanAirlinesContext>()
                .UseInMemoryDatabase("FlyingDutchman").Options;
        _context = new FlyingDutchmanAirlinesContext(dbContextOptions);

        Customer testCustomer = new Customer("John Doe");
        _context.Customers.Add(testCustomer);
        await _context.SaveChangesAsync();
        
        _repository = new CustomerRepository(_context);
        Assert.IsNotNull(_repository);
    }
    
    [TestMethod]
    public async Task CreateCustomer_Success()
    {
        var result = await _repository.CreateCustomer("John Doe");
        Assert.IsTrue(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_NameIsNull()
    {
        bool result = await _repository.CreateCustomer(null);
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_NameIsEmptyString()
    {
        bool result = await _repository.CreateCustomer("");
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow('#')]
    [DataRow('$')]
    [DataRow('%')]
    [DataRow('&')]
    [DataRow('*')]
    public async Task CreateCustomer_Failure_InvalidCharacter(char invalidChar)
    {
        bool result = await _repository.CreateCustomer($"someName{invalidChar}");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task CreateCustomer_Failure_DatabaseAccessError()
    {
        CustomerRepository repository = new CustomerRepository(null);
        bool result = await repository.CreateCustomer("John Doe");
        Assert.IsFalse(result);
    }

    [TestMethod]
    public async Task GetCustomerByName_Success()
    {
        Customer customer = await _repository.GetCustomerByName("John Doe");

        Customer dbCustomer = _context.Customers.First();
        
        Assert.AreEqual(customer, dbCustomer);
    }

    [TestMethod]
    [DataRow("")]
    [DataRow(null)]
    [DataRow("#")]
    [DataRow("$")]
    [DataRow("%")]
    [DataRow("&")]
    [DataRow("*")]
    [ExpectedException(typeof(CustomerNotFoundException))]
    public async Task GetCustomerByName_Failure_InvalidName(string name)
    {
        await _repository.GetCustomerByName(name);
    }
    
}