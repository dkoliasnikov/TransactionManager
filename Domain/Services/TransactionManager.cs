using Autofac;
using Domain.Abstractions;
using Domain.CQRS.Abstractions;
using Domain.CQRS.Abstractions.Params;
using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Models;

namespace Domain.Services;

internal class TransactionManager : ITransactionManager
{
	class Context
	{
		public State State { get; set; }
		public Context(State state)
		{
			this.State = state;
		}
		public void Request()
		{
			this.State.Handle(this);
		}
	}

	abstract class State
	{
		public abstract void Handle(Context context);
	}

	abstract class FetchCommandState : State
	{

	}

	abstract class InputCommandParameterStateState : State
	{

	}

	private readonly ILifetimeScope _scope;

	public TransactionManager(ILifetimeScope scope)
	{
		_scope = scope;
	}

	public async Task Run()
	{
		while (true)
		{
			Console.WriteLine("Введите команду");
			var command = Console.ReadLine();
			Type request = null;
			IParameter parameter = null;

			Dictionary<Type, Action<object>> map = new()
			{
				{
					typeof(IUserQuery<IGetTransactionParameter, Transaction>),
					(parameter) => {
						_scope.Resolve<IUserQuery<IGetTransactionParameter, Transaction>>().GetAsync(parameter as IGetTransactionParameter);
					}
				},
				{
					typeof(IUserCommand<IAddTransactionParameter>),
					(parameter) => {
						_scope.Resolve<IUserCommand<IAddTransactionParameter>>().Handle(parameter as IAddTransactionParameter);
					}
				},
				{
					typeof(IExitCommand),
					(parameter) => {
						_scope.Resolve<IExitCommand>().Handle(parameter as ExitAppParameter);
					}
				}
			};

			switch (command)
			{
				case "exit":
					request = typeof(IExitCommand);
					parameter = null;

					break;

				case "get":
					while (true)
					{
						Console.Write("Введите id");
						var _successfullyParsedId = int.TryParse(Console.ReadLine(), out var _id);
						if (!_successfullyParsedId)
							Console.WriteLine("Некорректное значение");
						else
							continue;

						request = typeof(IGetTransactionQueryHandler);
						parameter = new GetTransactionParameter(_id);

						break;
					}

					break;
				case "add":
					Console.Write("Введите id");
					var successfullyParsedId = int.TryParse(Console.ReadLine(), out var id);
					if (!successfullyParsedId)
						Console.WriteLine("Некорректное значение");
					else
						continue;

					Console.Write("Введите дату");
					var successfullyParsedDate = DateTime.TryParse(Console.ReadLine(), out var dateTime);
					if (!successfullyParsedDate)
						Console.WriteLine("Некорректное значение");
					else
						continue;

					Console.Write("Введите сумму");
					var successfullyParsedAmount = int.TryParse(Console.ReadLine(), out var amount);
					if (!successfullyParsedAmount)
						Console.WriteLine("Некорректное значение");
					else
						continue;

					request = typeof(IAddOrUpdateTransactionCommandHandler);
					parameter = new AddTransactionParameter(new Transaction(id, dateTime, amount));
					break;
				default:
					Console.WriteLine("Неизвестная команда");
					break;
			}

			map[request].Invoke(parameter);
		}
	}
}



