using System.ComponentModel.DataAnnotations;

namespace FlyingDutchmanAirlines.ControllerLayer.JsonData;

public class BookingData : IValidatableObject
{
    private string _firstName;
    private string _lastName;

    public string FirstName
    {
        get => _firstName;
        set => _firstName = ValidateName(value, nameof(FirstName));
    }

    public string LastName
    {
        get => _lastName;
        set => _lastName = ValidateName(value, nameof(LastName));
    }

    private string ValidateName(string name, string propertyName)
        => string.IsNullOrEmpty(name) 
            ? throw new InvalidOperationException("Could not set " + propertyName) 
            : name;


    public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        List<ValidationResult> results = new List<ValidationResult>();

        if (FirstName == null && LastName == null)
        {
            results.Add(new ValidationResult("All given data points are null"));
        } else if (FirstName == null || LastName == null)
        {
            results.Add(new ValidationResult("One of the given data points is null"));
        }

        return results;
    }
}