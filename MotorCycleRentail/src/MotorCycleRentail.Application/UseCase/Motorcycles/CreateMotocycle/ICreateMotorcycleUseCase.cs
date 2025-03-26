using MotorCycleRentail.Domain.Entities;

namespace MotorCycleRentail.Application.Usecase;

public interface ICreateMotorcycleUsecase : IUsecase
{
    Task<Motorcycle?> ExecuteAsync(Motorcycle request, CancellationToken ct = default);
}
