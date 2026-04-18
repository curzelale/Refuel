using Refuel.Domain.Exceptions;

namespace Refuel.Domain.Entities;

public class Refuel
{
    public Guid Id { get; private set; }
    public Guid VehicleId { get; private set; }
    public Guid GasStationId { get; private set; }
    public Guid FuelId { get; private set; }
    public double Quantity { get; private set; }
    public double TotalPrice { get; private set; }
    public DateTime Date { get; private set; }
    public float OdometerKm { get; private set;}
    public string? Note { get; private set; }
    public virtual Vehicle? Vehicle { get; private set; }
    public virtual GasStation? GasStation { get; private set; }
    public virtual Fuel? Fuel { get; private set; }

    //empty constructor is required for ef-core migrations
    public Refuel()
    {
        
    }

    public Refuel(Guid vehicleId, Guid gasStationId, Guid fuelId, double quantity, double totalPrice, DateTime date,
        float odometerKm, string? note)
    {
        EnforceVehicleIdBusinessRules(vehicleId);
        EnforceGasStationIdBusinessRules(gasStationId);
        EnforceFuelIdBusinessRules(fuelId);
        EnforceQuantityBusinessRules(quantity);
        EnforceTotalPriceBusinessRules(totalPrice);
        EnforceDateBusinessRules(date);
        EnforceOdometerKmBusinessRules(odometerKm);

        Id = Guid.NewGuid();
        VehicleId = vehicleId;
        GasStationId = gasStationId;
        FuelId = fuelId;
        Quantity = quantity;
        TotalPrice = totalPrice;
        Date = date;
        OdometerKm = odometerKm;
        Note = note;
    }

    private void EnforceVehicleIdBusinessRules(Guid vehicleId)
    {
        if (vehicleId == Guid.Empty)
        {
            throw new BusinessRuleException($"The {nameof(vehicleId)} cannot be empty.");
        }
    }

    private void EnforceGasStationIdBusinessRules(Guid gasStationId)
    {
        if (gasStationId == Guid.Empty)
        {
            throw new BusinessRuleException($"The {nameof(gasStationId)} cannot be empty.");
        }
    }

    private void EnforceFuelIdBusinessRules(Guid fuelId)
    {
        if (fuelId == Guid.Empty)
        {
            throw new BusinessRuleException($"The {nameof(fuelId)} cannot be empty.");
        }
    }

    private void EnforceQuantityBusinessRules(double quantity)
    {
        if (quantity <= 0)
        {
            throw new BusinessRuleException($"The {nameof(quantity)} must be greater than zero.");
        }
    }

    private void EnforceTotalPriceBusinessRules(double totalPrice)
    {
        if (totalPrice <= 0)
        {
            throw new BusinessRuleException($"The {nameof(totalPrice)} must be greater than zero.");
        }
    }

    private void EnforceDateBusinessRules(DateTime date)
    {
        //Add 5 minute to avoid time sync issue
        if (date > DateTime.UtcNow.AddMinutes(5))
        {
            throw new BusinessRuleException($"The {nameof(date)} cannot be in the future.");
        }
    }

    private void EnforceOdometerKmBusinessRules(float odometerKm)
    {
        if (odometerKm < 0)
        {
            throw new BusinessRuleException($"The {nameof(odometerKm)} cannot be negative.");
        }
    }
}