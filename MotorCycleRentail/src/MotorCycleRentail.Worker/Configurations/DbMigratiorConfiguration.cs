using Microsoft.EntityFrameworkCore;
using MotorCycleRentail.Infra.Persistence;

namespace MotorCycleRentail.Worker.Configurations;

public class DbMigrationConfigurator
{
    private readonly ILogger<DbMigrationConfigurator> _logger;

    public DbMigrationConfigurator(ILogger<DbMigrationConfigurator> logger)
    {
        _logger = logger;
    }

    public void ApplyMigrations(IServiceProvider serviceProvider)
    {
        try
        {
            using (var scope = serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<DataContext>(); // Substitua ApplicationDbContext pelo seu DbContext real
                dbContext.Database.Migrate(); // Aplica as migrations pendentes
                _logger.LogInformation("Migrations applied successfully.");
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while applying migrations.");
            throw; // Re-throw the exception if necessary
        }
    }
}
