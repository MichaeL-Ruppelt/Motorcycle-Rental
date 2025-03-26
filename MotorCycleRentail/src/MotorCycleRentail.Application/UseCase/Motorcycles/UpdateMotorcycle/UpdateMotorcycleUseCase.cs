
using MotorCycleRentail.Dto.Request;

namespace MotorCycleRentail.Application.Usecase;

public class UpdateMotorcycleUsecase : IUpdateMotorcycleUsecase
{
    #region ctor
    private readonly ILogger<UpdateMotorcycleUsecase> _logger;
    private readonly IMotorcycleRepository _motorcycleRepository;

    public UpdateMotorcycleUsecase(ILogger<UpdateMotorcycleUsecase> logger, IMotorcycleRepository motorcycleRepository)
    {
        _logger = logger;
        _motorcycleRepository = motorcycleRepository;
    }
    #endregion ctor

    public async Task<bool> ExecuteAsync(string id, UpdateMotorcycleRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(id))
        {
            _logger.LogError($"Id nulo ou vazio");
            return false;
        }

        if (!Guid.TryParse(id, out Guid idGuid))
        {
            _logger.LogError($"Id não é válido");
            return false;
        }

        if (string.IsNullOrEmpty(request.LicensePlate))
        {
            _logger.LogError($"LicensePlate is null or empty");
            return false;
        }

        var motorcycle = await _motorcycleRepository.GetByIdAsync(idGuid, ct);
        if (motorcycle is null)
            return false;

        motorcycle.LicensePlate = new string(request.LicensePlate.Where(c => char.IsLetterOrDigit(c)).ToArray());

        await _motorcycleRepository.UpdateAsync(motorcycle, ct);

        return true;
    }
}
