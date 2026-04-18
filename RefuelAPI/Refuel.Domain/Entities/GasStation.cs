using Refuel.Domain.Exceptions;

namespace Refuel.Domain.Entities;

public class GasStation
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }
    public string Address { get; private set; }
    public double Latitude { get; private set; }
    public double Longitude { get; private set; }
    public IEnumerable<Fuel> Fuels { get; private set; }

    //Empty constructor is required for ef-core migrations
    public GasStation()
    {
        
    }
    
    public GasStation(string name, string address, double latitude, double longitude)
    {
        EnforceNameBusinessRules(name);
        EnforceAddressBusinessRules(address);
        EnforceLatitudeBusinessRules(latitude);
        EnforceLongitudeBusinessRules(longitude);
        
        Name = name;
        Address = address;
        Latitude = latitude;
        Longitude = longitude;
        Id = Guid.NewGuid();
    }

    private void EnforceNameBusinessRules(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleException($"The {nameof(name)} is required.");
        }
    }

    private void EnforceAddressBusinessRules(string address)
    {
        if (string.IsNullOrWhiteSpace(address))
        {
            throw new BusinessRuleException($"The {nameof(address)} is required.");
        }
    }

    private void EnforceLatitudeBusinessRules(double latitude)
    {
        if (latitude is < -90 or > 90)
        {
            throw new BusinessRuleException($"The {nameof(latitude)} must be between -90 and 90.");
        }
    }

    private void EnforceLongitudeBusinessRules(double longitude)
    {
        if (longitude is < -180 or > 180)
        {
            throw new BusinessRuleException($"The {nameof(longitude)} must be between -180 and 180.");
        }
    }
}