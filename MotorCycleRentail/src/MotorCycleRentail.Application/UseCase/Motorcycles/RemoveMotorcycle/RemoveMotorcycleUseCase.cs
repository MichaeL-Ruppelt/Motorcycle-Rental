using MotorCycleRentail.Domain.Entities;

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

        var motorcycle = await _motorcycleRepository.GetByIdentifierAsync(id, ct);
        if (motorcycle is null)
            return false;

        await _motorcycleRepository.DeleteById(motorcycle.Id, ct);

        return true;
    }
}
