namespace MotorCycleRentail.Infra.Persistence.Sql.Repositories;

public class GenericRepositorySql<TEntity> : IGenericRepositorySql<TEntity> where TEntity : class, IBaseEntity
{
    #region Ctor
    private readonly DbContext _dbContext;

    public GenericRepositorySql(DbContext dbContext)
    {
        _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
    }

    private IExecutionStrategy CreateExecutionStrategy()
    {
        return _dbContext.Database.CreateExecutionStrategy();
    }
    #endregion Ctor

    public async Task InsertAsync(TEntity entity, CancellationToken ct)
    {
        await CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            await _dbContext.Set<TEntity>().AddAsync(entity);
            await _dbContext.SaveChangesAsync(ct);
        });
    }
    public async Task UpdateAsync(Guid id, TEntity entity, CancellationToken ct = default)
    {
        await CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            _dbContext.Set<TEntity>().Update(entity);
            await _dbContext.SaveChangesAsync(ct);
        });
    }
    public async Task DeleteAsync(Guid id, CancellationToken ct = default)
    {
        await CreateExecutionStrategy().ExecuteAsync(async () =>
        {
            TEntity entity = await GetByIdAsync(id);
            if (entity != null)
            {
                _dbContext.Set<TEntity>().Remove(entity);
                await _dbContext.SaveChangesAsync(ct);
            }
        });
    }


    public async Task<TEntity> GetByIdAsync(Guid id, CancellationToken ct = default)
    {
        return await _dbContext.Set<TEntity>().FirstOrDefaultAsync(e => e.Id == id, ct);
    }
    public async Task<TEntity?> GetByIdAsync(Guid id, CancellationToken ct = default, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsQueryable();
        foreach (Expression<Func<TEntity, object>> include in includes)
        {
            if (include.Body is MemberExpression memberExpression)
            {
                query = query.Include(memberExpression.Member.Name);
            }
        }

        return await query.AsNoTracking().FirstOrDefaultAsync(x => x.Id == id, ct);
    }
    public async Task<IEnumerable<TEntity>> GetAllAsync(CancellationToken ct = default)
    {
        return await _dbContext.Set<TEntity>().AsNoTracking().ToListAsync(ct);
    }
    public IQueryable<TEntity> GetAll(params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsQueryable();
        foreach (Expression<Func<TEntity, object>> include in includes)
        {
            if (include.Body is MemberExpression memberExpression)
            {
                query = query.Include(memberExpression.Member.Name);
            }
        }

        return query.AsNoTracking();
    }
    public IQueryable<TEntity> GetAll(Expression<Func<TEntity, bool>> filter, params Expression<Func<TEntity, object>>[] includes)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>().AsQueryable();
        foreach (Expression<Func<TEntity, object>> include in includes)
        {
            if (include.Body is MemberExpression memberExpression)
            {
                query = query.Include(memberExpression.Member.Name);
            }
        }

        return query.Where(filter).AsNoTracking();
    }

}

