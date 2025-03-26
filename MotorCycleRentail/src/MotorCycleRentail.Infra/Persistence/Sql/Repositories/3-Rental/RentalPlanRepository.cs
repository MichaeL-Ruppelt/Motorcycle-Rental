namespace MotorCycleRentail.Infra.Persistence.Sql.Repositories;

public class RentalPlanRepository : IRentalPlanRepository
{
    #region CTOR
    private readonly IGenericRepositorySql<RentalPlan> _genericRepository;

    public RentalPlanRepository(IDbContextAccessor<DataContext> dbContextAccessor)
    {
        _genericRepository = new GenericRepositorySql<RentalPlan>(dbContextAccessor.DbContext);
    }
    #endregion CTOR

    public async Task<RentalPlan?> GetByPlanDays(int days, CancellationToken ct = default)
    {
        return await _genericRepository.GetAll(x => x.PlanDays == days).FirstOrDefaultAsync(ct);

    }
}