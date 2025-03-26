namespace MotorCycleRentail.Application.Usecase;

public class CreateMotorcycleUsecase : ICreateMotorcycleUsecase
{
    #region ctor
    private readonly ILogger<CreateMotorcycleUsecase> _logger;
    private readonly IMotorcycleRepository _motorcycleRepository;

    public CreateMotorcycleUsecase(ILogger<CreateMotorcycleUsecase> logger, IMotorcycleRepository motorcycleRepository)
    {
        _logger = logger;
        _motorcycleRepository = motorcycleRepository;
    }
    #endregion ctor

    public async Task<Motorcycle?> ExecuteAsync(Motorcycle newMotorcycle, CancellationToken ct = default)
    {
        if(!IsDataValid(newMotorcycle))
            return null;

        if (await MotorcycleExist(newMotorcycle.LicensePlate, ct))
            return null;

        await _motorcycleRepository.InsertAsync(newMotorcycle, ct);
        _logger.LogInformation($"New Motorcycle created: Id: {newMotorcycle.Id}, Identifier: {newMotorcycle.Identifier}");

        return newMotorcycle;
    }

    private bool IsDataValid(Motorcycle newMotorcycle)
    {
        var dataValidation = new MotorcycleValidation().Validate(newMotorcycle);
        if (!dataValidation.IsValid)
        {
            _logger.LogWarning($"Invalid data: {string.Join(", ", dataValidation.Errors)}");
            return false;
        }

        return true;
    }
    private async Task<bool> MotorcycleExist(string licensePlate, CancellationToken ct)
    {
        var motorcycle = await _motorcycleRepository.GetByLicensePlate(licensePlate, ct);
        if (motorcycle is not null)
        {
            _logger.LogWarning($"Motorcycle with license plate: {licensePlate} already exists");
            return true;
        }

        return false;
    }


}
