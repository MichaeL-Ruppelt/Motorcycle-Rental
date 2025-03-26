using MotorCycleRentail.Worker.Configurations;

namespace MotorCycleRentail.Worker
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);
            builder.Services.AddCustomApp();
            builder.Services.AddCustomAutoMapper();
            builder.Services.AddCustomEntityFrameworkPostgres(builder.Configuration);
            builder.Services.ConfigureMassTransit(builder.Configuration);
            builder.Services.Configure<HostOptions>(options =>
            {
                //Determina o limite de tempo de empera ao finalizar um pod.
                options.ShutdownTimeout = TimeSpan.FromSeconds(60);
            });
            builder.AddErrorHandler();
            builder.Services.AddSingleton<DbMigrationConfigurator>();

            var host = builder.Build();

            var migrationConfigurator = host.Services.GetRequiredService<DbMigrationConfigurator>();
            migrationConfigurator.ApplyMigrations(host.Services);
            host.Run();
        }
    }
}