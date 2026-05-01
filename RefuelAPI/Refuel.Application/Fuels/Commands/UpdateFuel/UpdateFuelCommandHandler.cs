using Refuel.Application.Fuels.Dtos;
using Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Application.Fuels.Commands.UpdateFuel;

public class UpdateFuelCommandHandler : IRequestHandler<UpdateFuelCommand, FuelDto>
{
    private readonly IRepository<Fuel> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public UpdateFuelCommandHandler(IRepository<Fuel> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<FuelDto> Handle(UpdateFuelCommand request, CancellationToken cancellationToken = default)
    {
        var fuel = await _repository.GetByIdAsync(request.Id)
                   ?? throw new KeyNotFoundException($"Fuel with id '{request.Id}' was not found.");

        fuel.Update(request.Name);

        _repository.Update(fuel);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new FuelDto(fuel.Id, fuel.Name);
    }
}
