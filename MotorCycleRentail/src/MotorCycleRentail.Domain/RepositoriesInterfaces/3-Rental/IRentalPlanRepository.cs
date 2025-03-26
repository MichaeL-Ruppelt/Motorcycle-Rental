namespace MotorCycleRentail.Domain.RepositoriesInterfaces;

public interface IRentalPlanRepository : IRepository
{
    Task<RentalPlan?> GetByPlanDays(int days, CancellationToken ct = default);
}
