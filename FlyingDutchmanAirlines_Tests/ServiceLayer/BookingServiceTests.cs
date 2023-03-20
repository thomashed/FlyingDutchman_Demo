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
    
    [TestInitialize]
    public async Task TestInitialize()
    {
        
    }

    [TestMethod]
    public async Task CreateBooking_Success()
    {
        Mock<BookingRepository> mockBookingRepository = new Mock<BookingRepository>();
        Mock<CustomerRepository> mockCustomerRepository = new Mock<CustomerRepository>();

        mockBookingRepository.Setup(repository => 
                repository.CreateBooking(0, 0)).Returns(Task.CompletedTask);
        mockCustomerRepository.Setup(repository => 
            repository.GetCustomerByName("Leo Tolstoy")).Returns(Task.FromResult(new Customer("Leo Tolstoy")));
        
        BookingService service = 
            new BookingService(mockBookingRepository.Object, mockCustomerRepository.Object);

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
        Mock<BookingRepository> mockBookingRepository = new Mock<BookingRepository>();
        Mock<CustomerRepository> mockCustomerRepository = new Mock<CustomerRepository>();

        BookingService service = 
            new BookingService(mockBookingRepository.Object, mockCustomerRepository.Object);

        (bool result, Exception exception) = await service.CreateBooking(name, flightNumber);
        
        Assert.IsFalse(result);
        Assert.IsNotNull(exception);
    }
}