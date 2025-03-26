using MassTransit;
using MotorCycleRentail.Worker.Consumers;

namespace MotorCycleRentail.Worker.Configurations;

public static class MassTransitConfiguration
{
    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(masstransit =>
        {
            masstransit.AddConsumer<MotorcycleRegistrationConsumer>();

            var queueProvider = configuration["Masstransit:Provider"] ?? "";

            switch (queueProvider)
            {
                case "SERVICEBUS":
                    ConfigureServiceBus(masstransit, configuration);
                    break;
                case "ACTIVEMQ":
                    ConfigureActiveMq(masstransit, configuration);
                    break;
                case "AWSSQS":
                    ConfigureAmazonSqs(masstransit, configuration);
                    break;
                case "RABBITMQ":
                    ConfigureRabbitMq(masstransit, configuration);
                    break;
                default:
                    throw new InvalidOperationException($"Queue provider not supported. Provider[{queueProvider}]");
            }

        });
        return services;
    }

    private static void ConfigureServiceBus(IBusRegistrationConfigurator masstransit, IConfiguration configuration)
    {
        masstransit.UsingAzureServiceBus((context, config) =>
        {
            var host = configuration["Default:ConnectionStrings:ServiceBusConnection"] ?? "";

            config.Host(host);
            config.ClearSerialization();
            config.UseRawJsonSerializer(RawSerializerOptions.AnyMessageType);
            config.ConfigureJsonSerializerOptions(options =>
            {
                return options;
            });

            ConfigureReceiveEndpoints(context, config, configuration);
        });
    }
    private static void ConfigureActiveMq(IBusRegistrationConfigurator masstransit, IConfiguration configuration)
    {
        masstransit.UsingActiveMq((context, config) =>
        {
            var host = configuration["Amqp:HostName:Financialaudit"] ?? "";
            var port = configuration["Amqp:Port"] ?? "";
            var userName = configuration["Amqp:UserName"] ?? "";
            var password = configuration["Amqp:Password"] ?? "";

            config.Host(host, int.Parse(port), h =>
            {
                h.Username(userName);
                h.Password(password);
                h.UseSsl();
            });
            config.ClearSerialization();
            config.UseRawJsonSerializer(RawSerializerOptions.AnyMessageType);
            config.ConfigureJsonSerializerOptions(options =>
            {
                return options;
            });

            ConfigureReceiveEndpoints(context, config, configuration);
        });
    }
    private static void ConfigureAmazonSqs(IBusRegistrationConfigurator masstransit, IConfiguration configuration)
    {
        masstransit.UsingAmazonSqs((context, config) =>
        {
            var accessKey = configuration["AWSSQS:AccessKeyId"] ?? "";
            var secretKey = configuration["AWSSQS:SecretAccessKey"] ?? "";
            var region = configuration["AWSSQS:Region"] ?? "";

            config.Host("sqs." + region + ".amazonaws.com", h =>
            {
                h.AccessKey(accessKey);
                h.SecretKey(secretKey);
            });

            ConfigureReceiveEndpoints(context, config, configuration);
        });
    }
    private static void ConfigureRabbitMq(IBusRegistrationConfigurator masstransit, IConfiguration configuration)
    {
        masstransit.UsingRabbitMq((context, config) =>
        {
            var host = configuration["RabbitMq:ConnectionString"] ?? "";

            config.Host(host);
            ConfigureReceiveEndpoints(context, config, configuration);
        });
    }
    private static void ConfigureReceiveEndpoints(IBusRegistrationContext context, IBusFactoryConfigurator config, IConfiguration configuration)
    {
         var queueName = configuration["Masstransit:Queue"] ?? "";

        config.ReceiveEndpoint(queueName,
            cfg =>
            {
                cfg.ConfigureConsumeTopology = false;
                cfg.ConcurrentMessageLimit = configuration.GetValue<int>("Masstransit:ConcurrentMessageLimit");
                cfg.UseMessageRetry(retryConfig =>
                {
                    retryConfig.Incremental(3, TimeSpan.FromSeconds(1), TimeSpan.FromSeconds(2));
                });
                cfg.ConfigureConsumer<MotorcycleRegistrationConsumer>(context);
            });
    }
}
