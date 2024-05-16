using Domain.Abstractions;
using Domain.CQRS.Abstractions;

namespace Domain.Services;

internal class TransactionManager : ITransactionManager
{
	public TransactionManager()
	{
	}

	public async Task Run()
	{
		while (true)
		{
			Console.WriteLine("Введите команду");
			var command = Console.ReadLine();
			switch (command)
			{
				case "":

					break;
				default:
					Console.WriteLine("Неизвестная команда");
					break;
			}
		}
	}

	private IUserRequest GetRequest
}



