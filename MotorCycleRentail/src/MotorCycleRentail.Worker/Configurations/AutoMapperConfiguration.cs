using System.Diagnostics.CodeAnalysis;

namespace MotorCycleRentail.Worker.Configurations;

[ExcludeFromCodeCoverage]
public static class AutoMapperConfiguration
{
    public static IServiceCollection AddCustomAutoMapper(this IServiceCollection services)
    {
        services.AddAutoMapper(new[]
        {
            typeof(Application.AssemblyMarking).Assembly,
            typeof(Domain.AssemblyMarking).Assembly,
            typeof(Infra.AssemblyMarking).Assembly
        });

        return services;
    }
}
