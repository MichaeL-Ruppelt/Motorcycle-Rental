using Microsoft.AspNetCore.Mvc;
using MotorCycleRentail.Application.Usecase;
using MotorCycleRentail.Dto.Request;

namespace MotorCycleRentail.Api.Controllers;

[ApiController]
[Route("entregadores")]
public class CourierController : ControllerBase
{
    #region ctor
    private readonly ILogger<CourierController> _logger;
    private readonly ICreateCourierUsecase _createCourierUsecase;
    private readonly IUpdateDriverDocumentUseCase _receiveDriverDocumentUsecase;

    public CourierController(ILogger<CourierController> logger,
        ICreateCourierUsecase createCourierUsecase,
        IUpdateDriverDocumentUseCase receiveDriverDocumentUsecase)
    {
        _logger = logger;
        _createCourierUsecase = createCourierUsecase;
        _receiveDriverDocumentUsecase = receiveDriverDocumentUsecase;
    }
    #endregion ctor

    [HttpPost()]
    public async Task<IActionResult> Post([FromBody] CourierRequest request, CancellationToken ct)
    {
        if (await _createCourierUsecase.ExecuteAsync(request, ct))
            return Created("", null);

        return BadRequest(new { mensagem = "Dados inválidos" });
    }

    [HttpPost("{id}/cnh")]
    public async Task<IActionResult> Update([FromBody] UpdateCnhImageRequest request, string id, CancellationToken ct)
    {
        if (await _receiveDriverDocumentUsecase.ExecuteAsync(id, request.CnhImage, ct))
            return Created("", null);

        return BadRequest(new { mensagem = "Dados inválidos" });
    }

}