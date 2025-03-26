namespace MotorCycleRentail.Application.Usecase;

public interface ICreateCourierUsecase : IUsecase
{
    Task<bool> ExecuteAsync(CourierRequest request, CancellationToken ct = default);
}
