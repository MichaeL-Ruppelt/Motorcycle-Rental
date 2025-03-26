namespace MotorCycleRentail.Application.Usecase;

public interface IRemoveMotorcycleUsecase : IUsecase
{
    Task<bool> ExecuteAsync(string id, CancellationToken ct = default);
}
