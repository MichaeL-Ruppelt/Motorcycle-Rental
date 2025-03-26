namespace MotorCycleRentail.Domain.RepositoriesInterfaces;

public interface IMotorcycleRepository : IRepository
{
    Task<Motorcycle> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Motorcycle> GetByIdentifierAsync(string identifier, CancellationToken ct = default);
    Task<Motorcycle> GetByLicensePlate(string licensePlate, CancellationToken ct = default);
    Task<IEnumerable<Motorcycle>> GetAllAsync(CancellationToken ct = default);
    Task InsertAsync(Motorcycle entity, CancellationToken ct = default);
    Task UpdateAsync(Motorcycle entity, CancellationToken ct = default);
    Task DeleteById(Guid id, CancellationToken ct = default);
}
