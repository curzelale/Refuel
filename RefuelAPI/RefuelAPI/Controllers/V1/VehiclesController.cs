using Mediator;
using Microsoft.AspNetCore.Mvc;
using Refuel.Application.Refuels.Dtos;
using Refuel.Application.Refuels.Queries.GetRefuelsByVehicleId;
using Refuel.Application.Vehicles.Commands.CreateVehicle;
using Refuel.Application.Vehicles.Dtos;
using Refuel.Application.Vehicles.Queries.GetAllVehicles;
using Refuel.Application.Vehicles.Queries.GetVehicleById;
using RefuelAPI.Authorization;
using RefuelAPI.Controllers.V1.Requests;
using RefuelAPI.Models;

namespace RefuelAPI.Controllers.V1;

//TODO: Aggiungere endpoint di modifica veicolo
//TODO: Aggiungere endpoint per la cancellazione di un veicolo controllando che non sia associato ad altro
//TODO: Non permettere la creazione di veicoli con la stessa targa
/// <summary>
/// Manage your vehicles. Each vehicle has a brand, model, license plate, nickname, and supported fuel types.
/// </summary>
[ApiController]
[Route("api/v1/[controller]")]
[BearerAuthorize]
[Produces("application/json")]
public class VehiclesController : ControllerBase
{
    private readonly IMediator _mediator;

    public VehiclesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>Returns all vehicles registered in the system.</summary>
    /// <response code="200">List of all vehicles.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<VehicleDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetAllVehiclesQuery(), cancellationToken);
        return Ok(result);
    }

    /// <summary>Returns a single vehicle by its unique identifier.</summary>
    /// <param name="id">The vehicle's unique identifier.</param>
    /// <response code="200">The requested vehicle.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="404">No vehicle found with the given identifier.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("{id:guid}")]
    [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status404NotFound)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetById(Guid id, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetVehicleByIdQuery(id), cancellationToken);
        return result is null ? NotFound() : Ok(result);
    }

    /// <summary>Returns all refueling records associated with a specific vehicle.</summary>
    /// <param name="vehicleId">The vehicle's unique identifier.</param>
    /// <response code="200">List of refueling records for the vehicle.</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpGet("{vehicleId:guid}/refuels")]
    [ProducesResponseType(typeof(IEnumerable<RefuelDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> GetRefuels(Guid vehicleId, CancellationToken cancellationToken)
    {
        var result = await _mediator.Send(new GetRefuelsByVehicleIdQuery(vehicleId), cancellationToken);
        return Ok(result);
    }

    /// <summary>Registers a new vehicle.</summary>
    /// <param name="request">Vehicle creation payload.</param>
    /// <response code="201">Vehicle created successfully. Returns the created resource.</response>
    /// <response code="400">Invalid request data (e.g. missing required fields).</response>
    /// <response code="401">Missing or invalid authentication token.</response>
    /// <response code="500">An unexpected server error occurred.</response>
    [HttpPost]
    [Consumes("application/json")]
    [ProducesResponseType(typeof(VehicleDto), StatusCodes.Status201Created)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status400BadRequest)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status401Unauthorized)]
    [ProducesResponseType(typeof(ErrorResponse), StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Create([FromBody] CreateVehicleRequest request)
    {
        var command = new CreateVehicleCommand(request.Brand, request.Model, request.Owner, request.FuelIds ?? [],
            request.Nickname, request.LicencesPlate);
        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetById), new { id = result.Id }, result);
    }
}
