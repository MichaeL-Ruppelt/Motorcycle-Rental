namespace MotorCycleRentail.Application.Usecase;

public interface ICalculateRentalPriceUseCase : IUsecase
{
    Task<RentalResponse?> ExecuteAsync(string id, CancellationToken ct = default);
}
