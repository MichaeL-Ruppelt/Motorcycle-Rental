
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

    public async Task<bool> ExecuteAsync(string identifier, UpdateMotorcycleRequest request, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(identifier))
        {
            _logger.LogError($"Id nulo ou vazio");
            return false;
        }

        var motorcycle = await _motorcycleRepository.GetByIdentifierAsync(identifier, ct);
        if (motorcycle is null)
        {
            _logger.LogError($"Motorcicle not found. Identifier: {identifier}");
            return false;
        }

        if (string.IsNullOrEmpty(request.LicensePlate))
        {
            _logger.LogError($"LicensePlate is null or empty");
            return false;
        }

        string licensePlate = new string(request.LicensePlate.Where(c => char.IsLetterOrDigit(c)).ToArray());

        Motorcycle motorcicleWithLicencePlate = await _motorcycleRepository.GetByLicensePlate(licensePlate, ct);
        if (motorcicleWithLicencePlate is not null)
        {
            _logger.LogError($"LicensePlate already exist. LicencePlate: {request}");
            return false;
        }

        motorcycle.LicensePlate = licensePlate;

        await _motorcycleRepository.UpdateAsync(motorcycle, ct);

        return true;
    }
}
