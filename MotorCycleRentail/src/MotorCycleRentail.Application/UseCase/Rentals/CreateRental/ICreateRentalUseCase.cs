

namespace MotorCycleRentail.Application.Usecase;

public interface ICreateRentalUseCase : IUsecase
{
    Task<bool> ExecuteAsync(RentalRequest request, CancellationToken ct = default);
}
