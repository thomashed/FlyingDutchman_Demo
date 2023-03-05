using FlyingDutchmanAirlines.RepositoryLayer;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class CustomerRepositoryTests
{
    [TestMethod]
    public void CreateCustomer_Success()
    {
        CustomerRepository repository =  new CustomerRepository();
        Assert.IsNotNull(repository);
    }
}