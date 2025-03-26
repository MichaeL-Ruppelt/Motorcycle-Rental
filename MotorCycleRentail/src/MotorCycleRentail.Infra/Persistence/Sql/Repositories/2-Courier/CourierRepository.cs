
namespace MotorCycleRentail.Infra.Persistence.Sql.Repositories;

public class CourierRepository : ICourierRepository
{
    #region CTOR
    private readonly IGenericRepositorySql<Courier> _genericRepository;

    public CourierRepository(IDbContextAccessor<DataContext> dbContextAccessor)
    {
        _genericRepository = new GenericRepositorySql<Courier>(dbContextAccessor.DbContext);
    }
    #endregion CTOR

    public async Task<Courier> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _genericRepository.GetByIdAsync(id, ct, x => x.Id);
    }
    public async Task<Courier> GetByIdentifierAsync(string identifier, CancellationToken ct = default)
    {
        return await _genericRepository.GetAll(x => x.Identifier == identifier).FirstOrDefaultAsync(ct);
    }
    public async Task<bool> CheckIfCourierAlreadyExist(string cnhNumber, string cnpj, CancellationToken ct = default)
    {
        return await _genericRepository.GetAll(x => (x.CnhNumber == cnhNumber) || (x.Cnpj == cnpj)).AnyAsync(ct);
    }

    public async Task InsertAsync(Courier entity, CancellationToken ct = default)
    {
        await _genericRepository.InsertAsync(entity, ct);
    }
    public async Task UpdateAsync(Courier entity, CancellationToken ct = default)
    {
        entity.UpdateAt = DateTime.Now;
        await _genericRepository.UpdateAsync(entity.Id, entity, ct);
    }

}

