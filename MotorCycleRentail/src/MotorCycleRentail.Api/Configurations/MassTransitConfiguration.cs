using MassTransit;
using Microsoft.VisualBasic;

namespace MotorCycleRentail.Api.Configurations;

public static class MassTransitConfiguration
{
    public static IServiceCollection ConfigureMassTransit(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddMassTransit(masstransit =>
        {
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
            var host = configuration["Masstransit:ServiceBus:Connectionstring"] ?? "";

            config.Host(host);
            config.ClearSerialization();
            config.UseRawJsonSerializer(RawSerializerOptions.AnyMessageType);
            config.ConfigureJsonSerializerOptions(options =>
            {
                return options;
            });

        });
    }
    private static void ConfigureActiveMq(IBusRegistrationConfigurator masstransit, IConfiguration configuration)
    {
        masstransit.UsingActiveMq((context, config) =>
        {
            var host = configuration["Amqp:HostName"] ?? "";
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

        });
    }
    private static void ConfigureRabbitMq(IBusRegistrationConfigurator masstransit, IConfiguration configuration)
    {
        masstransit.UsingRabbitMq((context, config) =>
        {
            var host = configuration["RabbitMq:ConnectionString"] ?? "";

            config.Host(host);
        });
    }

}
