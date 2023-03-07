using FlyingDutchmanAirlines.RepositoryLayer;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace FlyingDutchmanAirlines_Tests.RepositoryLayer;

[TestClass]
public class CustomerRepositoryTests
{
    [TestMethod]
    public void CreateCustomer_Success()
    {
        CustomerRepository repository =  new CustomerRepository();
        Assert.IsNotNull(repository);

        bool result =  repository.CreateCustomer("John Doe");
        Assert.IsTrue(result);
    }

    [TestMethod]
    public void CreateCustomer_Failure_NameIsNull()
    {
        CustomerRepository repository = new CustomerRepository();
        Assert.IsNotNull(repository);
        bool result = repository.CreateCustomer(null);
        
        Assert.IsFalse(result);
    }

    [TestMethod]
    public void CreateCustomer_Failure_NameIsEmptyString()
    {
        CustomerRepository repository = new CustomerRepository();
        Assert.IsNotNull(repository);
        bool result = repository.CreateCustomer("");
        
        Assert.IsFalse(result);
    }

    [TestMethod]
    [DataRow('#')]
    [DataRow('$')]
    [DataRow('%')]
    [DataRow('&')]
    [DataRow('*')]
    public void CreateCustomer_Failure_InvalidCharacter(char invalidChar)
    {
        CustomerRepository repository = new CustomerRepository();
        Assert.IsNotNull(repository);

        bool result = repository.CreateCustomer($"someName{invalidChar}");
        Assert.IsFalse(result);
    }
    
}