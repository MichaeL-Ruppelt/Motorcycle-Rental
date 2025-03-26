using MotorCycleRentail.Infra.Persistence.Sql.Contexts.Mapping;

namespace MotorCycleRentail.Infra.Persistence;

public class DataContext : DbContext
{
    private readonly IConfiguration _configuration;

    public DataContext(DbContextOptions<DataContext> options, IConfiguration configuration) : base(options)
    {
        _configuration = configuration;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new MotorcycleMapping(_configuration));
        modelBuilder.ApplyConfiguration(new CourierMapping(_configuration));
        modelBuilder.ApplyConfiguration(new RentalMapping(_configuration));
        modelBuilder.ApplyConfiguration(new RentalPlanMapping(_configuration));

        // Aplica filtro de exclusão lógica para todas as entidades que possuem a propriedade IsDeleted
        foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        {
            if (entityType.ClrType.GetProperty("IsDeleted") != null)
            {
                var method = typeof(DataContext)
                    .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)
                    ?.MakeGenericMethod(entityType.ClrType);

                method?.Invoke(null, new object[] { modelBuilder });
            }
        }

        foreach (var property in GetStringProperties(modelBuilder))
            property.SetIsUnicode(false);

        base.OnModelCreating(modelBuilder);
    }

    private static void SetSoftDeleteFilter<T>(ModelBuilder modelBuilder) where T : class
    {
        modelBuilder.Entity<T>().HasQueryFilter(e => !EF.Property<bool>(e, "IsDeleted"));
    }

    private static IEnumerable<IMutableProperty> GetStringProperties(ModelBuilder modelBuilder)
    {
        return modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(string) && p.GetColumnType() == null);
    }

    public DbSet<Motorcycle> Motorcycles => Set<Motorcycle>();
    public DbSet<Courier> Couriers => Set<Courier>();
    public DbSet<Rental> Rentals => Set<Rental>();
    public DbSet<RentalPlan> RentalsPlans => Set<RentalPlan>();
}
