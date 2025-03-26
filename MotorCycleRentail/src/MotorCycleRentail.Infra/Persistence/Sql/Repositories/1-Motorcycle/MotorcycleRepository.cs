namespace MotorcycleRentail.Infra.Persistence.Sql.Repositories;

public class MotorcycleRepository : IMotorcycleRepository
{
    #region CTOR
    private readonly IGenericRepositorySql<Motorcycle> _genericRepository;

    public MotorcycleRepository(IDbContextAccessor<DataContext> dbContextAccessor)
    {
        _genericRepository = new GenericRepositorySql<Motorcycle>(dbContextAccessor.DbContext);
    }

    public async Task DeleteById(Guid id, CancellationToken ct = default)
    {
        await _genericRepository.DeleteAsync(id);
    }

    public async Task<IEnumerable<Motorcycle>> GetAllAsync(CancellationToken ct = default)
    {
        return await _genericRepository.GetAllAsync();
    }

    public async Task<Motorcycle> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _genericRepository.GetByIdAsync(id, ct, x => x.Id);
    }

    public async Task<Motorcycle> GetByIdentifierAsync(string identifier, CancellationToken ct = default)
    {
        return await _genericRepository.GetAll(x => x.Identifier == identifier).FirstOrDefaultAsync(ct);
    }

    public async Task<Motorcycle> GetByLicensePlate(string licensePlate, CancellationToken ct = default)
    {
        return await _genericRepository.GetAll(x => x.LicensePlate == licensePlate).FirstOrDefaultAsync(ct);
    }

    public async Task InsertAsync(Motorcycle entity, CancellationToken ct = default)
    {
        await _genericRepository.InsertAsync(entity, ct);
    }

    public async Task UpdateAsync(Motorcycle entity, CancellationToken ct = default)
    {
        await _genericRepository.UpdateAsync(entity.Id, entity, ct);
    }

    #endregion CTOR




}
