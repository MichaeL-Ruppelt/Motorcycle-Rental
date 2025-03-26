
namespace MotorCycleRentail.Infra.Persistence.Sql.Repositories;

public class RentalRepository : IRentalRepository
{
    #region CTOR
    private readonly IGenericRepositorySql<Rental> _genericRepository;

    public RentalRepository(IDbContextAccessor<DataContext> dbContextAccessor)
    {
        _genericRepository = new GenericRepositorySql<Rental>(dbContextAccessor.DbContext);
    }
    #endregion CTOR

    public async Task<Rental> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _genericRepository.GetByIdAsync(id, ct);
    }

    public async Task InsertAsync(Rental entity, CancellationToken ct = default)
    {
        await _genericRepository.InsertAsync(entity, ct);
    }
    public async Task UpdateAsync(Rental entity, CancellationToken ct = default)
    {
        entity.UpdateAt = DateTime.Now;
        await _genericRepository.UpdateAsync(entity.Id, entity, ct);
    }
}
