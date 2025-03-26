using MassTransit;
using MassTransit.Transports;
using MotorCycleRentail.Application.Usecase;
using MotorCycleRentail.Domain.Entities;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Text.Json;

namespace MotorCycleRentail.Worker.Consumers;

public class MotorcycleRegistrationConsumer : IConsumer<Motorcycle>
{
    private readonly ILogger<MotorcycleRegistrationConsumer> _logger;
    private readonly ICreateMotorcycleUsecase _createMotocycleUsecase;
    private readonly ISendEndpointProvider _sendEndpointProvider;

    public MotorcycleRegistrationConsumer(ILogger<MotorcycleRegistrationConsumer> logger,
                                          ICreateMotorcycleUsecase createMotocycleUsecase,
                                          ISendEndpointProvider sendEndpointProvider)
    {
        _logger = logger;
        _createMotocycleUsecase = createMotocycleUsecase;
        _sendEndpointProvider = sendEndpointProvider;
    }

    public async Task Consume(ConsumeContext<Motorcycle> context)
    {
        _logger.LogInformation($"Message received at: {DateTime.UtcNow}, type of: {nameof(Motorcycle)}");

        Motorcycle newMotorcyle = ParseMessage(context);

        if (newMotorcyle is null)
        {
            _logger.LogWarning($"Message data is empty: {DateTime.UtcNow}, type of: {nameof(Motorcycle)}");
            await SendToErrorQueueAsync(context);
            return;
        }

        var processResult = await _createMotocycleUsecase.ExecuteAsync(newMotorcyle, context.CancellationToken);
        if (processResult is null)
        {
            _logger.LogWarning($"Unable to process message data: {DateTime.UtcNow}, type of: {nameof(Motorcycle)}, sending to error queue.");
            await SendToErrorQueueAsync(context);
        }
    }

    #region Helpers

    /// <summary>
    ///  Retorna a mensagem do envelope do MassTransit e deserializa corretamente para o objeto Motorcycle.
    /// </summary>
    private Motorcycle ParseMessage(ConsumeContext<Motorcycle> context)
    {
        var streamMessage = context.ReceiveContext.Body.GetBytes();
        var jsonString = Encoding.UTF8.GetString(streamMessage);

        JObject jsonObject = JObject.Parse(jsonString);

        if (jsonObject.ContainsKey("message"))
        {
            var messageJsonValue = jsonObject["message"].ToString();
            return JsonConvert.DeserializeObject<Motorcycle>(messageJsonValue);
        }

        return JsonConvert.DeserializeObject<Motorcycle>(jsonString);
    }

    /// <summary>
    /// Envia a mensagem para a fila de erro no padrão "{original-queue-name}_error".
    /// </summary>
    private async Task SendToErrorQueueAsync(ConsumeContext<Motorcycle> context)
    {
        var errorQueueName = $"{context.ReceiveContext.InputAddress.AbsolutePath}_error";
        var errorQueue = await _sendEndpointProvider.GetSendEndpoint(new Uri($"queue:{errorQueueName}"));  // URL da fila de erro
        await errorQueue.Send(context.Message);  // Enviar a mensagem original para a fila de erro
    }

    #endregion Helpers
}
