using Refuel.Application.Fuels.Dtos;
using Mediator;
using Refuel.Application.UnitOfWork;
using Refuel.Domain.Entities;
using Refuel.Domain.Repositories;

namespace Refuel.Application.Fuels.Commands.CreateFuel;

public class CreateFuelCommandHandler : IRequestHandler<CreateFuelCommand, FuelDto>
{
    private readonly IRepository<Fuel> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public CreateFuelCommandHandler(IRepository<Fuel> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async ValueTask<FuelDto> Handle(CreateFuelCommand request, CancellationToken cancellationToken = default)
    {
        var fuel = new Fuel(request.Name);

        await _repository.AddAsync(fuel);
        await _unitOfWork.CommitAsync(cancellationToken);

        return new FuelDto(fuel.Id, fuel.Name);
    }
}
