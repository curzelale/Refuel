using Refuel.Domain.Exceptions;

namespace Refuel.Domain.Entities;

public class Fuel
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    //empty constructor is required for ef-core migrations
    public Fuel()
    {
        
    }

    public Fuel(string name)
    {
        EnforceNameBusinessRules(name);
        Name = name;
        Id = Guid.NewGuid();
    }
    
    
    private void EnforceNameBusinessRules(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
        {
            throw new BusinessRuleException($"The {nameof(name)} is required.");
        }
    }
}