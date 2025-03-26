using MotorCycleRentail.Common.Interfaces;
using System.Diagnostics.CodeAnalysis;

namespace MotorCycleRentail.Api.Configurations;

[ExcludeFromCodeCoverage]
public static class AppConfiguration
{
    /// <summary>
    /// Injeta os serviços de modo dinâmico através do assembly.
    /// </summary>
    /// <param name="services"></param>
    public static void AddCustomApp(this IServiceCollection services)
    {
        services.Scan(scan => scan
            .FromAssemblyOf<Application.AssemblyMarking>()
                //Register Usecases
                .AddClasses(classes => classes.AssignableTo<IUsecase>())
                    .AsImplementedInterfaces(i => i != typeof(IUsecase))
                    .WithScopedLifetime()
                //Register Services
                .AddClasses(classes => classes.AssignableTo<IService>())
                    .AsImplementedInterfaces(i => i != typeof(IService))
                    .WithScopedLifetime()
            .FromAssembliesOf(typeof(Infra.AssemblyMarking))
                .AddClasses(classes => classes.AssignableTo<IRepository>())
                    .AsImplementedInterfaces(i => i != typeof(IRepository))
                    .WithScopedLifetime()
        );
    }
}
