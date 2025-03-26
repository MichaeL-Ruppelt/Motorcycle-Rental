using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using MotorCycleRentail.Application.Usecase;
using MotorCycleRentail.Domain.RepositoriesInterfaces;
using MotorCycleRentail.Dto.Request;
using MotorCycleRentail.Dto.Response.Motorcycle;


namespace MotorCycleRentail.Api.Controllers;

[ApiController]
[Route("motos")]
public class MotocycleController : ControllerBase
{
    #region ctor
    private readonly ILogger<MotocycleController> _logger;
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly ISendMotocycleEventUseCase _sendMotorcycleEventUseCase;
    private readonly IRemoveMotorcycleUsecase _removeMotorcycleUseCase;
    private readonly IUpdateMotorcycleUsecase _updateMotorcycleUseCase;
    private readonly IMapper _mapper;

    public MotocycleController(ILogger<MotocycleController> logger,
        IMotorcycleRepository motorcycleRepository,
        ISendMotocycleEventUseCase sendMotorcycleEventUseCase,
        IRemoveMotorcycleUsecase removeMotorcycleUseCase,
        IUpdateMotorcycleUsecase updateMotorcycleUseCase,
        IMapper mapper)
    {
        _logger = logger;
        _motorcycleRepository = motorcycleRepository;
        _sendMotorcycleEventUseCase = sendMotorcycleEventUseCase;
        _removeMotorcycleUseCase = removeMotorcycleUseCase;
        _updateMotorcycleUseCase = updateMotorcycleUseCase;
        _mapper = mapper;
    }
    #endregion ctor

    [HttpPost()]
    public async Task<IActionResult> Post([FromBody] MotorcycleRequest request, CancellationToken ct)
    {
        if(await _sendMotorcycleEventUseCase.ExecuteAsync(request, ct))
            return Created("", null);

        return BadRequest(new { mensagem = "Dados inválidos" });
    }

    [HttpGet()]
    public async Task<IActionResult> GetAll([FromQuery] string? id, CancellationToken ct)
    {
        if (!string.IsNullOrEmpty(id))
        {
            var motorcycle = await _motorcycleRepository.GetByIdentifierAsync(id);
            if(motorcycle is null)
                return NotFound(new { mensagem = "Moto não encontrada" });
            return Ok(new[] { _mapper.Map<MotorcycleResponse>(motorcycle) });
        }

        var motorcycles = await _motorcycleRepository.GetAllAsync(ct);
        if (motorcycles?.Any() != true)
            return NotFound(new { mensagem = "Motos não encontradas" });

        var result = motorcycles.Select(m => _mapper.Map<MotorcycleResponse>(m));
        return Ok(result);

    }

    [HttpPut("{id}/placa")]
    public async Task<IActionResult> Update([FromBody] UpdateMotorcycleRequest request, string id, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest(new { mensagem = "Request mal formada" });

        if (await _updateMotorcycleUseCase.ExecuteAsync(id, request, ct))
            return Ok(new { mensagem = "Placa modificada com sucesso" });

        return BadRequest(new { mensagem = "Dados inválidos" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(string id)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest(new { mensagem = "Request mal formada" });

        var motorcycle = await _motorcycleRepository.GetByIdentifierAsync(id);

        if (motorcycle is null)
            return NotFound(new { mensagem = "Moto não encontrada" });

        return Ok(_mapper.Map<MotorcycleResponse>(motorcycle));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest(new { mensagem = "Request mal formada" });

        if (await _removeMotorcycleUseCase.ExecuteAsync(id, ct))
            return Ok();

        return BadRequest(new { mensagem = "Dados inválidos" });
    }
}
