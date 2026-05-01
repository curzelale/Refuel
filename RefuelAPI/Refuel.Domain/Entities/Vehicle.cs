using Refuel.Domain.Exceptions;

namespace Refuel.Domain.Entities;

public class Vehicle
{
    public Guid Id { get; private set; }
    public string Brand { get; private set; }
    public string Model { get; private set; }
    public string Owner { get; private set; }
    
    public string? Nickname { get; private set; }
    public string? LicencesPlate { get; private set; }
    public ICollection<Fuel> Fuels { get; private set; } = new List<Fuel>();

    //Empty constructor is required for ef-core migrations
    public Vehicle()
    {
    }

    public Vehicle(string brand, string model, string owner, string? nickname, string? licencesPlate)
    {
        EnforceBrandBusinessRules(brand);
        EnforceModelBusinessRules(model);
        EnforceOwnerBusinessRules(owner);

        Id = Guid.NewGuid();
        Brand = brand;
        Model = model;
        Owner = owner;
        Nickname = nickname;
        LicencesPlate = licencesPlate;
    }

    public void AddFuel(Fuel fuel)
    {
        if (Fuels.Any(f => f.Id == fuel.Id))
            return;
        Fuels.Add(fuel);
    }

    private void EnforceBrandBusinessRules(string brand)
    {
        if (string.IsNullOrWhiteSpace(brand))
        {
            throw new BusinessRuleException($"The {nameof(brand)} is required.");
        }
    }

    private void EnforceModelBusinessRules(string model)
    {
        if (string.IsNullOrWhiteSpace(model))
        {
            throw new BusinessRuleException($"The {nameof(model)} is required.");
        }
    }

    private void EnforceOwnerBusinessRules(string owner)
    {
        if (string.IsNullOrWhiteSpace(owner))
        {
            throw new BusinessRuleException($"The {nameof(owner)} is required.");
        }
    }
}