using Autofac;
using Domain.Abstractions;
using Domain.CQRS.Abstractions;
using Domain.CQRS.Abstractions.Params;
using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Models;
using System.Text.Json;
using System.Text.Json.Serialization;

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

	Dictionary<Type, Action<object>> pesudoMediatr ;

	private readonly ILifetimeScope _scope;

	public TransactionManager(ILifetimeScope scope)
	{
		_scope = scope;
		
		pesudoMediatr = new()
			{
				{
					typeof(IGetTransactionQueryHandler), (parameter) => Console.WriteLine(JsonSerializer.Serialize(_scope.Resolve<IGetTransactionQueryHandler>().GetAsync(parameter as IGetTransactionParameter)))
				},
				{
					typeof(IAddOrUpdateTransactionCommandHandler), (parameter) => _scope.Resolve<IAddOrUpdateTransactionCommandHandler>().Handle(parameter as IAddTransactionParameter)

				},
				{
					typeof(IExitCommand), (parameter) => _scope.Resolve<IExitCommand>().Handle(parameter as ExitAppParameter)
				}
			};
	}


	public async Task Run()
	{
		while (true)
		{
			try
			{
				Console.WriteLine("Введите команду");
				var command = Console.ReadLine();
				Type request = null;
				IParameter parameter = null;

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
							{
								Console.WriteLine("Некорректное значение");
								continue;
							}

							request = typeof(IGetTransactionQueryHandler);
							parameter = new GetTransactionParameter(_id);

							break;
						}

						break;

					case "add":
						Console.Write("Введите id");
						var successfullyParsedId = int.TryParse(Console.ReadLine(), out var id);
						if (!successfullyParsedId)
						{
							Console.WriteLine("Некорректное значение");
							continue;
						}

						Console.Write("Введите дату");
						var successfullyParsedDate = DateTime.TryParse(Console.ReadLine(), out var dateTime);
						if (!successfullyParsedDate)
						{
							Console.WriteLine("Некорректное значение");
							continue;
						}

						Console.Write("Введите сумму");
						var successfullyParsedAmount = int.TryParse(Console.ReadLine(), out var amount);
						if (!successfullyParsedAmount)
						{
							Console.WriteLine("Некорректное значение");
							continue;
						}

						request = typeof(IAddOrUpdateTransactionCommandHandler);
						parameter = new AddTransactionParameter(new Transaction(id, dateTime, amount));
						break;
					default:
						Console.WriteLine("Неизвестная команда");
						break;
				}

				pesudoMediatr[request].Invoke(parameter);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}



