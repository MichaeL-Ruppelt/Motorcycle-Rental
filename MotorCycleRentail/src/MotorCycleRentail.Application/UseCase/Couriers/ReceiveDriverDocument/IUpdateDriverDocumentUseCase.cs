using MotorCycleRentail.Dto.Request;

namespace MotorCycleRentail.Application.Usecase
{
    public interface IUpdateDriverDocumentUseCase : IUsecase
    {
        Task<bool> ExecuteAsync(string courierId, string newCnhImage, CancellationToken ct = default);
    }
}
