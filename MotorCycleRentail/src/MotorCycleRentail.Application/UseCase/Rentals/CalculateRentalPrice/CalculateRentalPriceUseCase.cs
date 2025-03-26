namespace MotorCycleRentail.Application.Usecase;

public class CalculateRentalPriceUseCase : ICalculateRentalPriceUseCase
{
    #region Ctor
    private readonly ILogger<CalculateRentalPriceUseCase> _logger;
    private readonly IRentalRepository _rentalRepository;
    private readonly IRentalPlanRepository _rentalPlanRepository;
    private static decimal  _overtakeFineValue;
    public CalculateRentalPriceUseCase(ILogger<CalculateRentalPriceUseCase> logger,
                                       IRentalRepository rentalRepository,
                                       IRentalPlanRepository rentalPlanRepository,
                                       IConfiguration configuration)
    {
        _logger = logger;
        _rentalRepository = rentalRepository;
        _rentalPlanRepository = rentalPlanRepository;
        _overtakeFineValue = SetOvertakeValue(configuration);
    }
    #endregion Ctor

    public async Task<RentalResponse?> ExecuteAsync(string id, CancellationToken ct = default)
    {
        if (!Guid.TryParse(id, out Guid idGuid))
        {
            _logger.LogError($"Id não é válido");
            return null;
        }

        var rental = await _rentalRepository.GetByIdAsync(idGuid, ct);
        if (rental is null)
        { 
            _logger.LogWarning($"Rental not found. RentalId {id}");
            return null;
        }

        var rentalPlan = await _rentalPlanRepository.GetByPlanDays(rental.PlanDays, ct);
        if (rentalPlan is null)
        {
            _logger.LogWarning($"Rental Plan not found. RentalId {id}, Rental Plan: {rental.PlanDays}");
            return null;
        }

        decimal rentalPrice = CalculateRentalPrice(rental, rentalPlan);

        return BuildRentalResponse(rental, rentalPrice);
    }

    #region Auxiliary Methods
    private RentalResponse BuildRentalResponse(Rental rental, decimal rentalPrice)
    {
        return new RentalResponse
        {
            Identifier = rental.Id.ToString(),
            RentalPrice = rentalPrice,
            CourierIdentifier = rental.CourierIdentifier,
            MotorcycleIdentifier = rental.MotorcycleIdentifier,
            StartDate = rental.StartDate,
            EndDate = rental.EndDate,
            ExpectedEndDate = rental.ExpectedEndDate,
            ReturnDate = rental.ReturnDate
        };
    }
    private decimal CalculateRentalPrice(Rental rental, RentalPlan rentalPlan)
    {
        if (rental.ReturnDate is null)
            return CalculateStandartPrice(rental, rentalPlan);

        return CalculateReturnPrice(rental, rentalPlan);
    }
    private static decimal CalculateStandartPrice(Rental rental, RentalPlan rentalPlan)
    {
        int totalDays = (rental.ExpectedEndDate - rental.StartDate).Days;
        decimal dailyRate = rentalPlan.PlanValue;
        return Math.Round(totalDays * dailyRate, 2);
    }
    private static decimal CalculateReturnPrice(Rental rental, RentalPlan rentalPlan)
    {
        int totalDays = (rental.ReturnDate.Value - rental.StartDate).Days;
        decimal dailyRate = rentalPlan.PlanValue;
        decimal totalCost = Math.Round(totalDays * dailyRate, 2);

        if (rental.ReturnDate < rental.ExpectedEndDate) //early return.
        {
            var remainingDays = (rental.ExpectedEndDate - rental.ReturnDate.Value).Days;
            decimal fineValue = rentalPlan.FineValue;
            totalCost += Math.Round(remainingDays * fineValue, 2);
        }

        if (rental.ReturnDate > rental.ExpectedEndDate) //late return.
        {
            var overtakeDays = (rental.ReturnDate.Value - rental.ExpectedEndDate).Days;
            totalCost += Math.Round(overtakeDays * _overtakeFineValue, 2);
        }

        return totalCost;
    }
    private decimal SetOvertakeValue(IConfiguration configuration)
    {
        if (configuration is not null)
        {
            var overtakeValue = configuration["BusinessRules:OvertakeFineValue"];
            if (decimal.TryParse(overtakeValue, out decimal value))
                return value;
        }
        return 0;
    }
    #endregion Auxiliary Methods
}
