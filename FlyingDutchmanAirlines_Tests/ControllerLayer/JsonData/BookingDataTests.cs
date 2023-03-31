using FlyingDutchmanAirlines.ControllerLayer.JsonData;
using Microsoft.AspNetCore.WebUtilities;

namespace FlyingDutchmanAirlines_Tests.ControllerLayer.JsonData;

[TestClass]
public class BookingDataTests
{
    [TestInitialize]
    public async Task TestInitialize()
    {
      
    }

    [TestMethod]
    public void BookingData_ValidData()
    {
        BookingData bookingData = new BookingData()
        {
            FirstName = "John",
            LastName = "Doe"
        };
        
        Assert.AreEqual("John", bookingData.FirstName);
        Assert.AreEqual("Doe", bookingData.LastName);
    }
    
    [TestMethod]
    [DataRow("Mike", null)]
    [DataRow(null, "Doe")]
    [ExpectedException(typeof(InvalidOperationException))]
    public void BookingData_InvalidData_NullPointers(string firstName, string lastName)
    {
        BookingData bookingData = new BookingData()
        {
            FirstName = firstName,
            LastName = lastName
        };
    }
    
    [TestMethod]
    [DataRow("Mike", "")]
    [DataRow("", "Doe")]
    [ExpectedException(typeof(InvalidOperationException))]
    public void BookingData_InvalidData_EmptyStrings(string firstName, string lastName)
    {
        BookingData bookingData = new BookingData()
        {
            FirstName = firstName,
            LastName = lastName
        };
        
        Assert.AreEqual("", bookingData.FirstName ?? "");
        Assert.AreEqual("", bookingData.LastName ?? "");
    }
    
}