using MotorCycleRentail.Dto.Request;

namespace MotorCycleRentail.Application.Usecase;

public interface IUpdateMotorcycleUsecase : IUsecase
{
    Task<bool> ExecuteAsync(string id, UpdateMotorcycleRequest request, CancellationToken ct = default);
}
