using MotorCycleRentail.Dto.Request;

namespace MotorCycleRentail.Application.Usecase;

public class CreateRentalUseCase : ICreateRentalUseCase
{
    #region Ctor
    private readonly ILogger<CreateRentalUseCase> _logger;
    private readonly ICourierRepository _courierRepository;
    private readonly IRentalRepository _rentalRepository;
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IRentalPlanRepository _rentalPlanRepository;
    public CreateRentalUseCase(ILogger<CreateRentalUseCase> logger,
                               ICourierRepository courierRepository,
                               IRentalRepository rentalRepository,
                               IMotorcycleRepository motorcycleRepository,
                               IRentalPlanRepository rentalPlanRepository)
    {
        _logger = logger;
        _courierRepository = courierRepository;
        _rentalRepository = rentalRepository;
        _motorcycleRepository = motorcycleRepository;
        _rentalPlanRepository = rentalPlanRepository;
    }
    #endregion Ctor

    public async Task<bool> ExecuteAsync(RentalRequest request, CancellationToken ct = default)
    {
        if (!await CourierExist(request.CourierIdentifier, ct))
            return false;

        if (!await MotorcycleExist(request.MotorcycleIdentifier, ct))
            return false;

        if (!await RentalPlanIsValid(request.PlanDays, ct))
            return false;

        if (!IsValidRentalDate(request))
            return false;

        var newRental = BuildNewRental(request);

        if (!IsDataValid(newRental))
            return false;

        await _rentalRepository.InsertAsync(newRental, ct);

        return true;
    }

    #region Auxiliar Methods
    private async Task<bool> MotorcycleExist(string motorcycleIdentifier, CancellationToken ct)
    {
        var motorcycle = await _motorcycleRepository.GetByIdentifierAsync(motorcycleIdentifier, ct);
        if (motorcycle is not null)
        {
            _logger.LogWarning($"Motorcycle not found. MotorcycleIdentifier {motorcycleIdentifier}");
            return true;
        }

        return false;
    }
    private async Task<bool> CourierExist(string courierIdentifier, CancellationToken ct)
    {
        var motorcycle = await _courierRepository.GetByIdentifierAsync(courierIdentifier, ct);
        if (motorcycle is null)
        {
            _logger.LogWarning($"Courier not found. CourierIdentifier {courierIdentifier}");
            return false;
        }

        return true;
    }
    private async Task<bool> RentalPlanIsValid(int planDays, CancellationToken ct)
    {
        var rentalPlan = await _rentalPlanRepository.GetByPlanDays(planDays, ct);
        if (rentalPlan is null)
        {
            _logger.LogWarning($"Rental Plan not found. Plan Days {planDays}");
            return false;
        }

        return true;
    }
    private bool IsValidRentalDate(RentalRequest rental)
    {
        DateTime tomorrow = DateTime.UtcNow.Date.AddDays(1);

        if (rental.StartDate.Date > tomorrow)
        {
            _logger.LogWarning($"StartDate is not valid. DataInicio: {rental.StartDate}");
            return false;
        }

        if (rental.EndDate.Date < tomorrow || rental.EndDate.Date < rental.StartDate.Date)
        {
            _logger.LogWarning($"EndDate is not valid. DataTermino: {rental.EndDate}");
            return false;
        }

        if (rental.ExpectedEndDate.Date < tomorrow || rental.ExpectedEndDate.Date < rental.StartDate.Date)
        {
            _logger.LogWarning($"ExpectedEndDate is not valid. DataPrevisaoTermino: {rental.ExpectedEndDate}");
            return false;
        }

        return true;
    }
    private static Rental BuildNewRental(RentalRequest request)
    {
        return new Rental
        {
            CourierIdentifier = request.CourierIdentifier,
            MotorcycleIdentifier = request.MotorcycleIdentifier,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            ExpectedEndDate = request.ExpectedEndDate,
            PlanDays = request.PlanDays,
        };
    }
    private bool IsDataValid(Rental newRental)
    {
        var dataValidation = new RentalValidation().Validate(newRental);
        if (!dataValidation.IsValid)
        {
            _logger.LogWarning($"Invalid data: {string.Join(", ", dataValidation.Errors)}");
            return false;
        }

        return true;
    }
    #endregion Auxiliar Methods
}
