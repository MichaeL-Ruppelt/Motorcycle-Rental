namespace MotorCycleRentail.Infra.Persistence.Sql.Repositories;

public interface IGenericRepositorySql<TEntity> where TEntity : class, IBaseEntity
{
    Task InsertAsync(TEntity entity, CancellationToken ct = default);
    Task UpdateAsync(Guid id, TEntity entity, CancellationToken ct = default);
    Task DeleteAsync(Guid id, CancellationToken ct = default);


    Task<TEntity> GetByIdAsync(Guid id, CancellationToken ct);
    Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes);
    Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default);
    IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes);
    IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes);


}
