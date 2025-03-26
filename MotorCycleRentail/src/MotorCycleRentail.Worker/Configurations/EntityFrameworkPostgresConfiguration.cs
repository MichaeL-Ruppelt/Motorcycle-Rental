using Microsoft.EntityFrameworkCore;
using MotorCycleRentail.Common.Interfaces;
using MotorCycleRentail.Infra.Persistence.DbContextAccessor;
using MotorCycleRentail.Infra.Persistence;
using System.Diagnostics.CodeAnalysis;


namespace MotorCycleRentail.Worker.Configurations;

[ExcludeFromCodeCoverage]
public static class EntityFrameworkPostgresConfiguration
{
    public static IServiceCollection AddCustomEntityFrameworkPostgres(this IServiceCollection services, IConfiguration configuration)
    {
        //Secret
        var postgresConnectionString = configuration["ConnectionStrings:PostgresConnection"] ?? "";

        //Migrations
        var migrationsAssemblyName = typeof(Infra.AssemblyMarking).Assembly.GetName().Name;

        //DbContext
        services.AddDbContext<DataContext>(x => x
            .UseNpgsql(postgresConnectionString, options => options
                .MigrationsHistoryTable("__EFMigrationsHistory")
                .MigrationsAssembly(migrationsAssemblyName)
                .EnableRetryOnFailure()
            ), ServiceLifetime.Scoped);

        services.AddScoped<IDbContextAccessor<DataContext>>(sp => new DbContextAccessor<DataContext>(() => sp.GetRequiredService<DataContext>()));

        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);

        return services;
    }




}
