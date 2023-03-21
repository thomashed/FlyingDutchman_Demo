using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.AspNetCore.Http;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.ExceptionServices;
using FlyingDutchmanAirlines_Tests.RepositoryLayer.Stubs;
using FlyingDutchmanAirlines.DatabaseLayer;
using FlyingDutchmanAirlines.DatabaseLayer.Models;
using FlyingDutchmanAirlines.Exceptions;
using FlyingDutchmanAirlines.ServiceLayer;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;
using Moq;

namespace FlyingDutchmanAirlines_Tests.ServiceLayer;

[TestClass]
public class BookingServiceTests
{
    private Mock<BookingRepository> _mockBookingRepository;
    private Mock<CustomerRepository> _mockCustomerRepository;

    [TestInitialize]
    public async Task TestInitialize()
    {
        _mockBookingRepository = new Mock<BookingRepository>();
        _mockCustomerRepository = new Mock<CustomerRepository>();
    }

    [TestMethod]
    public async Task CreateBooking_Success()
    {
        _mockBookingRepository.Setup(repository => 
                repository.CreateBooking(0, 0)).Returns(Task.CompletedTask);
        _mockCustomerRepository.Setup(repository => 
            repository.GetCustomerByName("Leo Tolstoy")).Returns(Task.FromResult(new Customer("Leo Tolstoy")));
        
        BookingService service = 
            new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object);

        (bool result, Exception exception) = await service.CreateBooking("Leo Tolstoy", 0);
        Assert.IsTrue(result);
        Assert.IsNull(exception);
    }

    [TestMethod]
    [DataRow("", 0)]
    [DataRow(null, -1)]
    [DataRow("Galileo Galilei", -1)]
    public async Task CreateBooking_Failure_InvalidInputArguments(string name, int flightNumber)
    {
        BookingService service = 
            new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object);

        (bool result, Exception exception) = await service.CreateBooking(name, flightNumber);
        
        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
    }

    [TestMethod]
    public async Task CreateBooking_Failure_RepositoryException_ArgumentException()
    {
        _mockBookingRepository.Setup(repository =>
            repository.CreateBooking(0, 1)).Throws(new ArgumentException());
    
        _mockCustomerRepository.Setup(repository => repository.GetCustomerByName("Galileo Galilei"))
            .Returns(Task.FromResult(new Customer("Galileo Galilei"){CustomerId = 0}));
        
        BookingService service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object);
        
        (bool result, Exception exception) = await service.CreateBooking("Galileo Galilei", 1);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsInstanceOfType(exception, typeof(ArgumentException));
    }

    [TestMethod]
    public async Task CreateBooking_Failure_RepositoryException_CouldNotAddBookingToDatabaseException()
    {
        _mockBookingRepository.Setup(repository => 
            repository.CreateBooking(1, 2)).Throws(new CouldNotAddBookingToDatabaseException());
        _mockCustomerRepository.Setup(repository => repository.GetCustomerByName("Eise Eisinga"))
            .Returns(Task.FromResult(new Customer("Eise Eisinga") { CustomerId = 1 }));

        BookingService service = new BookingService(_mockBookingRepository.Object, _mockCustomerRepository.Object);
        
        (bool result, Exception exception) = await service.CreateBooking("Eise Eisinga", 2);

        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
        Assert.IsTrue(exception is CouldNotAddBookingToDatabaseException);
    }
}