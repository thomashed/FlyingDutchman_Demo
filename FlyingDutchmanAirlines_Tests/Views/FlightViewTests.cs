using FlyingDutchmanAirlines.Views;

namespace FlyingDutchmanAirlines_Tests.Views;

[TestClass]
public class FlightViewTests
{

    [TestInitialize]
    public async Task TestInitialize()
    {
    }
 
    [TestMethod]
    public void Constructor_FlightView_Success() { 
        string flightNumber = "0";
        string originCity = "Amsterdam";
        string originCityCode = "AMS";
        string destinationCity = "Copenhagen";
        string destinationCityCode = "CPH";
        
        FlightView view = new FlightView(
            flightNumber, 
            (originCity, originCityCode), 
            (destinationCity, destinationCityCode));
        
        Assert.IsNotNull(view);
        Assert.AreEqual(view.FlightNumber, flightNumber);
        Assert.AreEqual(view.Origin.City, originCity);
        Assert.AreEqual(view.Origin.Code, originCityCode);
        Assert.AreEqual(view.Destination.City, destinationCity);
        Assert.AreEqual(view.Destination.Code, destinationCityCode);
    }

    [TestMethod]
    public void Constructor_Success_FlightNumber_Null()
    {
        string flightNumber = null;
        string originCity = "Athens";
        string originCityCode = "ATH";
        string destinationCity = "Dubai";
        string destinationCityCode = "DXB";
        
        FlightView view = new FlightView(null,
            (originCity, originCityCode),
            (destinationCity, destinationCityCode));
        
        Assert.IsNotNull(view);
        Assert.AreEqual(view.FlightNumber, "no flight-number found");
        Assert.AreEqual(view.Origin.City, originCity);
        Assert.AreEqual(view.Origin.Code, originCityCode);
        Assert.AreEqual(view.Destination.City, destinationCity);
        Assert.AreEqual(view.Destination.Code, destinationCityCode);
    }
    
    [TestMethod]
    public void Constructor_AirportInfo_Success_City_EmptyString() {
        string destinationCity = string.Empty;
        string destinationCityCode = "SYD";
        AirportInfo airportInfo = 
            new AirportInfo((destinationCity, destinationCityCode));
        Assert.IsNotNull(airportInfo);
        Assert.AreEqual(airportInfo.City, "No city found");
        Assert.AreEqual(airportInfo.Code, destinationCityCode);
    }

    [TestMethod]
    public void Constructor_AirportInfo_Success_Code_EmptyString()
    {
        string destinationCity = "Ushuaia";
        string destinationCityCode = string.Empty;
        AirportInfo airportInfo =
            new AirportInfo((destinationCity, destinationCityCode));
        Assert.IsNotNull(airportInfo);
        Assert.AreEqual(airportInfo.City, destinationCity);
        Assert.AreEqual(airportInfo.Code, "No code found");
    }
    
}