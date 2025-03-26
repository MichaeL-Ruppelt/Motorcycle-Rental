using Microsoft.EntityFrameworkCore;

namespace MotorCycleRentail.Common.Interfaces;

public interface IDbContextAccessor<TDbContext> where TDbContext : DbContext
{
    TDbContext DbContext { get; }

    void TransactionReadCommitted(Action<ITransactionScopeCommand> operation);
    TResult TransactionReadCommitted<TResult>(Func<ITransactionScopeCommand, TResult> operation);
    void TransactionReadUncommitted(Action<ITransactionScopeCommand> operation);
    TResult TransactionReadUncommitted<TResult>(Func<ITransactionScopeCommand, TResult> operation);
}
