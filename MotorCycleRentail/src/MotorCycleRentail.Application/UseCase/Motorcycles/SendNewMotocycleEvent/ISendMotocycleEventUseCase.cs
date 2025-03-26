namespace MotorCycleRentail.Application.Usecase;

public interface ISendMotocycleEventUseCase : IUsecase
{
    Task<bool> ExecuteAsync(MotorcycleRequest request, CancellationToken ct = default);
}
