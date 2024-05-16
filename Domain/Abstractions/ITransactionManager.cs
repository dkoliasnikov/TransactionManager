namespace Domain.Abstractions;

public interface ITransactionManager
{
	Task Run();
}