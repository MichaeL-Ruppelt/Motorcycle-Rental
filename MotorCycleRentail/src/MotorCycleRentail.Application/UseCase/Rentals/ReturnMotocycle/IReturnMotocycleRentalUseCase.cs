namespace MotorCycleRentail.Application.Usecase;

public interface IReturnMotocycleRentalUsecase : IUsecase
{
    Task<bool> ExecuteAsync(string id, DateTime newReturnDate, CancellationToken ct);
}
