namespace MotorCycleRentail.Domain.RepositoriesInterfaces;

public interface IRentalRepository : IRepository
{
    Task<Rental> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task InsertAsync(Rental entity, CancellationToken ct = default);
    Task UpdateAsync(Rental entity, CancellationToken ct = default);
}
