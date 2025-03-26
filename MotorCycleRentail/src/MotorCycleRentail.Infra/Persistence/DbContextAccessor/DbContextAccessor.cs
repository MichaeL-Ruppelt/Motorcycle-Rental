using MotorCycleRentail.Common.Interfaces;
using System.Transactions;

namespace MotorCycleRentail.Infra.Persistence.DbContextAccessor;

public class DbContextAccessor<TDbContext> : IDbContextAccessor<TDbContext> where TDbContext : DbContext
{
    private readonly Func<TDbContext> _factory;
    private readonly Stack<TDbContext> _contexts = new();

    public TDbContext DbContext
    {
        get
        {
            EnsureContextNotEmpty();
            return _contexts.Peek();
        }
    }

    public DbContextAccessor(Func<TDbContext> factory)
    {
        _factory = factory;
    }

    public void TransactionReadCommitted(Action<ITransactionScopeCommand> operation) =>
        TransactionScope(operation, IsolationLevel.ReadCommitted);

    public TResult TransactionReadCommitted<TResult>(Func<ITransactionScopeCommand, TResult> operation) =>
        TransactionScope(operation, IsolationLevel.ReadCommitted);

    public void TransactionReadUncommitted(Action<ITransactionScopeCommand> operation) =>
        TransactionScope(t => operation(t), IsolationLevel.ReadUncommitted);

    public TResult TransactionReadUncommitted<TResult>(Func<ITransactionScopeCommand, TResult> operation) =>
        TransactionScope(operation, IsolationLevel.ReadUncommitted);

    private TResult TransactionScope<TResult>(Func<ITransactionScopeCommand, TResult> operation, IsolationLevel isolationLevel)
    {
        TResult result = default;
        TransactionScope((Action<ITransactionScopeCommand>)(t => result = operation(t)), isolationLevel);
        return result;
    }

    private void TransactionScope(Action<ITransactionScopeCommand> operation, IsolationLevel isolationLevel) =>
        DbContext.Database
            .CreateExecutionStrategy()
            .Execute(() => Execute(operation, isolationLevel));

    private void Execute(Action<ITransactionScopeCommand> operation, IsolationLevel isolationLevel)
    {
        var transactionOptions = new TransactionOptions { IsolationLevel = isolationLevel };
        using var transaction = new TransactionScope(TransactionScopeOption.Required, transactionOptions);
        try
        {
            using var context = EnlistNewDbContext();
            operation(new TransactionScopeCommand(transaction));
        }
        finally
        {
            DiscardDbContext();
        }
    }

    private void EnsureContextNotEmpty()
    {
        if (_contexts.Count == 0)
            EnlistNewDbContext();
    }

    private TDbContext EnlistNewDbContext()
    {
        var dbContext = _factory();
        _contexts.Push(dbContext);
        return dbContext;
    }

    private void DiscardDbContext() =>
        _contexts.Pop();
    
}

