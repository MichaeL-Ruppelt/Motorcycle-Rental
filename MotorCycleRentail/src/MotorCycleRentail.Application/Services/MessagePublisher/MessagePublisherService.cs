using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace MotorCycleRentail.Application.Services;

public class MessagePublisherService : IMessagePublisherService
{
    private readonly IBus _bus;
    private readonly ILogger<MessagePublisherService> _logger;
    private string _queueName;

    public MessagePublisherService(IBus bus, ILogger<MessagePublisherService> logger, IConfiguration configuration)
    {
        _bus = bus;
        _logger = logger;
        _queueName = configuration["Masstransit:Queue"] ?? "";
    }

    /// <summary>
    /// Send message to queue
    /// </summary>
    /// <param name="message"> object data send to queue </param>
    /// <param name="ct"> cancelation token </param>
    public async Task SendMessage<T>(T message, CancellationToken ct)
    {
        if (message is null)
            throw new InvalidOperationException($"Sem dados para envio de mensagem");

        _logger.LogInformation($"Envio de mensagem, {message.GetType().Name}");

        var publisher = await _bus.GetSendEndpoint(new Uri($"queue:{_queueName}"));
        await publisher.Send(message, ct);
    }
}
