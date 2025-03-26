namespace MotorCycleRentail.Application.Usecase;

public class RemoveMotorcycleUsecase : IRemoveMotorcycleUsecase
{
    #region ctor
    private readonly ILogger<RemoveMotorcycleUsecase> _logger;
    private readonly IMotorcycleRepository _motorcycleRepository;

    public RemoveMotorcycleUsecase(ILogger<RemoveMotorcycleUsecase> logger, IMotorcycleRepository motorcycleRepository)
    {
        _logger = logger;
        _motorcycleRepository = motorcycleRepository;
    }
    #endregion ctor

    public async Task<bool> ExecuteAsync(string id, CancellationToken ct = default)
    {
        if (string.IsNullOrEmpty(id))
            return false;

        if (!Guid.TryParse(id, out Guid idGuid))
            return false;

        var motorcycle = await _motorcycleRepository.GetByIdAsync(idGuid, ct);
        if (motorcycle is null)
            return false;

        await _motorcycleRepository.DeleteById(idGuid, ct);

        return true;
    }
}
