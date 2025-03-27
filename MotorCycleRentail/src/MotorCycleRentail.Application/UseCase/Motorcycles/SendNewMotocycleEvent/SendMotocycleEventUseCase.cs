namespace MotorCycleRentail.Application.Usecase;

public class SendMotocycleEventUseCase : ISendMotocycleEventUseCase
{
    #region ctor
    private readonly ILogger<SendMotocycleEventUseCase> _logger;
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IMessagePublisherService _messagePublisher;
    private int _maxMotorcycleYear;

    public SendMotocycleEventUseCase(
        ILogger<SendMotocycleEventUseCase> logger, 
        IMotorcycleRepository motorcycleRepository,
        IMessagePublisherService messagePublisher, 
        IConfiguration configuration)
    {
        _logger = logger;
        _motorcycleRepository = motorcycleRepository;
        _messagePublisher = messagePublisher;
        _maxMotorcycleYear = SetMaxMotorcycleYear(configuration);
    }
    #endregion ctor

    public async Task<bool> ExecuteAsync(MotorcycleRequest request, CancellationToken ct = default)
    {
        if (request.Year < _maxMotorcycleYear)
        {
            _logger.LogWarning($"Invalid Motorcycle year: {request.Year}, max year: {_maxMotorcycleYear}");
            return false;
        }

        var newMotorcycle = BuildNewMotorcycle(request);

        if (!IsDataValid(newMotorcycle))
            return false;

        if (await MotorcycleExist(request.LicensePlate, request.Identifier, ct))
            return false;

        await _messagePublisher.SendMessage(newMotorcycle, ct);

        return true;
    }

    #region Auxiliar Methods
    private int SetMaxMotorcycleYear(IConfiguration configuration)
    {
        if (configuration is not null)
        {
            var maxMotorcycleYear = configuration["BusinessRules:MaxMotorcycleYear"] ?? "0";
            if (int.TryParse(maxMotorcycleYear, out int year))
                return year;
        }
        return 0;
    }
    private static Motorcycle BuildNewMotorcycle(MotorcycleRequest request)
    {
        return new Motorcycle
        {
            Identifier = request?.Identifier?.Trim() ?? "",
            Year = request?.Year ?? 0,
            Model = request?.Model?.Trim() ?? "",
            LicensePlate = new string(request?.LicensePlate?.Where(c => char.IsLetterOrDigit(c)).ToArray()) ?? ""
        };
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
    private async Task<bool> MotorcycleExist(string licensePlate, string identifier, CancellationToken ct)
    {
        var motorcycle = await _motorcycleRepository.GetByLicensePlate(licensePlate, ct);
        if (motorcycle is not null)
        {
            _logger.LogWarning($"Motorcycle with license plate {licensePlate} already exists");
            return true;
        }

        motorcycle = await _motorcycleRepository.GetByIdentifierAsync(identifier, ct);
        if (motorcycle is not null)
        {
            _logger.LogWarning($"Motorcycle with identifier {identifier} already exists");
            return true;
        }

        return false;
    }
    #endregion Auxiliar Methods
}
