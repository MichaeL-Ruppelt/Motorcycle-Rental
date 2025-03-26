
namespace MotorCycleRentail.Application.Services;

public interface IMessagePublisherService : IService
{
    Task SendMessage<T>(T message, CancellationToken ct);
}
