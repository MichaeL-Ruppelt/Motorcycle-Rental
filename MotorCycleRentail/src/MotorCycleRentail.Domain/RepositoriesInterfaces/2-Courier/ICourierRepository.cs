namespace MotorCycleRentail.Domain.RepositoriesInterfaces;

public interface ICourierRepository : IRepository
{
    Task<Courier> GetByIdAsync(Guid id, CancellationToken ct = default);
    Task<Courier> GetByIdentifierAsync(string identifier, CancellationToken ct = default);
    Task<bool> CheckIfCourierAlreadyExist(string cnhNumber, string cnpj, CancellationToken ct = default);
    Task InsertAsync(Courier entity, CancellationToken ct = default);
    Task UpdateAsync(Courier entity, CancellationToken ct = default);
}
