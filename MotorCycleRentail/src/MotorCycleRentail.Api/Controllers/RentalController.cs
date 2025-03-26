using Microsoft.AspNetCore.Mvc;
using MotorCycleRentail.Application.Usecase;
using MotorCycleRentail.Dto.Request;

namespace MotorCycleRentail.Api.Controllers;

[ApiController]
[Route("locacao")]
public class RentalController : ControllerBase
{
    #region ctor
    private readonly ICreateRentalUseCase _createRentalUsecase;
    private readonly IReturnMotocycleRentalUsecase _returnRentalUsecase;
    private readonly ICalculateRentalPriceUseCase _calculateRentalPriceUseCase;

    public RentalController(ICreateRentalUseCase createRentalUsecase,
                            IReturnMotocycleRentalUsecase returnRentalUsecase,
                            ICalculateRentalPriceUseCase calculateRentalPriceUseCase)
    {
        _createRentalUsecase = createRentalUsecase;
        _returnRentalUsecase = returnRentalUsecase;
        _calculateRentalPriceUseCase = calculateRentalPriceUseCase;
    }
    #endregion ctor

    [HttpPost()]
    public async Task<IActionResult> Post([FromBody] RentalRequest request, CancellationToken ct)
    {
        if (await _createRentalUsecase.ExecuteAsync(request, ct))
            return Created("", null);

        return BadRequest(new { mensagem = "Dados inválidos" });
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetOne(string id)
    {
        if (string.IsNullOrEmpty(id))
            return BadRequest(new { mensagem = "Request mal formada" });

        var rentalCalculated = await _calculateRentalPriceUseCase.ExecuteAsync(id);

        if (rentalCalculated is null)
            return NotFound(new { mensagem = "Locação não encontrada" });

        return Ok(rentalCalculated);
    }

    [HttpPut("{id}/devolucao")]
    public async Task<IActionResult> UpdateReturnDate([FromBody] RentalUpdateReturnRequest request, string id, CancellationToken ct)
    {
        if (string.IsNullOrEmpty(id) || request?.ReturnDate is null)
            return BadRequest(new { mensagem = "Request mal formada" });

        if (await _returnRentalUsecase.ExecuteAsync(id, request.ReturnDate, ct))
            return Ok(new { mensagem = "Data de devolução informada com sucesso" });

        return BadRequest(new { mensagem = "Dados inválidos" });
    }




}


