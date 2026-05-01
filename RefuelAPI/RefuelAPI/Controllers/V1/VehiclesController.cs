using Mediator;
using Microsoft.AspNetCore.Mvc;
using Refuel.Application.Vehicles.Commands.CreateVehicle;
using Refuel.Application.Vehicles.Queries.GetAllVehicles;
using Refuel.Application.Vehicles.Queries.GetVehicleById;
using RefuelAPI.Controllers.V1.Requests;

namespace RefuelAPI.Controllers.V1;

//TODO: Aggiungere endpoint di modifica veicolo
//TODO: Aggiungere endpoint per la cancellazione di un veicolo controllando che non sia associato ad altro
//TODO: Non permettere la creazione di veicoli con la stessa targa
[ApiController]
[Route("api/v1/{controller}")]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllVehiclesQuery(), cancellationToken);
        return Ok(result);
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVehicleByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
    {
        var command = new CreateVehicleCommand(request.Brand, request.Model, request.Owner, request.FuelIds ?? [],
            request.Nickname, request.LicencesPlate);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}