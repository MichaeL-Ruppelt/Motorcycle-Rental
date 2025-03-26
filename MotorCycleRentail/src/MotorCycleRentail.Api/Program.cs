
using MotorCycleRentail.Api.Configurations;

namespace MotorCycleRentail.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddCustomApp();
        builder.Services.AddCustomAutoMapper();
        builder.Services.AddCustomEntityFrameworkPostgres(builder.Configuration);
        builder.Services.ConfigureMassTransit(builder.Configuration);
        builder.Services.AddSwaggerGen();
        builder.Services.Configure<HostOptions>(options =>
        { 
            //Determina o limite de tempo de empera ao finalizar um pod.
            options.ShutdownTimeout = TimeSpan.FromSeconds(60);
        });
        builder.Services.AddSingleton<DbMigrationConfigurator>();

        var app = builder.Build();

        var migrationConfigurator = app.Services.GetRequiredService<DbMigrationConfigurator>();
        migrationConfigurator.ApplyMigrations(app.Services);

        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseErrorHandler();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}
