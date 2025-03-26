
namespace MotorCycleRentail.Application.Usecase;

public class ReturnMotocycleRentalUsecase : IReturnMotocycleRentalUsecase
{
    #region Ctor
    private readonly ILogger<ReturnMotocycleRentalUsecase> _logger;
    private readonly IRentalRepository _rentalRepository;
    public ReturnMotocycleRentalUsecase(ILogger<ReturnMotocycleRentalUsecase> logger, IRentalRepository rentalRepository)
    {
        _logger = logger;
        _rentalRepository = rentalRepository;
    }
    #endregion Ctor

    public async Task<bool> ExecuteAsync(string id, DateTime newReturnDate, CancellationToken ct)
    {
        if (!Guid.TryParse(id, out Guid idGuid))
        {
            _logger.LogWarning($"Id não é válido: {id}");
            return false;
        }

        var rental = await _rentalRepository.GetByIdAsync(idGuid, ct);
        if (rental is null)
        {
            _logger.LogWarning($"locação não encontrada. Id:{id}");
            return false;
        }

        if(rental.StartDate > newReturnDate)
        {
            _logger.LogWarning($"Data de devolução Inválida:{newReturnDate}, deve ser maior que a data de início: {rental.StartDate}");
            return false;
        }

        rental.ReturnDate = newReturnDate;
        await _rentalRepository.UpdateAsync(rental, ct);

        return true;
    }
}
