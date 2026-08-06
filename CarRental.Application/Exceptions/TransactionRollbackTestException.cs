namespace CarRental.Application.Exceptions;

public class TransactionRollbackTestException : Exception
{
    public TransactionRollbackTestException()
        : base("Rollback test exception.")
    {
    }
}