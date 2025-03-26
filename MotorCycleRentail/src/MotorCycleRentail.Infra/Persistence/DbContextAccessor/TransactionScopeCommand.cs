using System.Transactions;

namespace MotorCycleRentail.Infra.Persistence.DbContextAccessor;

public class TransactionScopeCommand : ITransactionScopeCommand
{
    private readonly TransactionScope _transactionScope;

    public TransactionScopeCommand(TransactionScope transactionScope)
    {
        _transactionScope = transactionScope;
    }

    public void Complete() => _transactionScope.Complete();
}
