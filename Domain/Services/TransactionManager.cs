using Autofac;
using Domain.Abstractions;
using Domain.CQRS.Abstractions;
using Domain.CQRS.Abstractions.Params;
using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Models;
using Generic.CQRS.Abstractions.Params.Abstractions;
using Generic.Exceptions;
using System.Text.Json;

namespace Domain.Services;

internal class TransactionManager : ITransactionManager
{
	private readonly Dictionary<Type, Func<object, Task>> _handlersMap ;

	private readonly ILifetimeScope _scope;

	public TransactionManager(ILifetimeScope scope)
	{
		_scope = scope;
		
		_handlersMap = new()
			{
				{
					typeof(IGetTransactionQueryHandler), async (parameter) => Console.WriteLine(JsonSerializer.Serialize(await _scope.Resolve<IGetTransactionQueryHandler>().GetAsync(parameter as IGetTransactionParameter)))
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
				Console.WriteLine("Введите команду ");
				var command = Console.ReadLine();
				Type? request = null;
				IParameter? parameter = null;

				switch (command)
				{
					case "exit":
						request = typeof(IExitCommand);
						parameter = null;

						break;

					case "get":
						while (true)
						{
							Console.Write("Введите id ");
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
						Console.Write("Введите id ");
						var successfullyParsedId = int.TryParse(Console.ReadLine(), out var id);
						if (!successfullyParsedId)
						{
							Console.WriteLine("Некорректное значение");
							continue;
						}

						Console.Write("Введите дату ");
						var successfullyParsedDate = DateTime.TryParse(Console.ReadLine(), out var dateTime);
						if (!successfullyParsedDate)
						{
							Console.WriteLine("Некорректное значение");
							continue;
						}

						Console.Write("Введите сумму ");
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

				if (request is not null && parameter is not null)
				{ 
					await _handlersMap[request].Invoke(parameter);
					request = null;
					parameter = null;
					Console.WriteLine("[Ok]");
				}

			}
			catch (EntityNotFoundException ex)
			{
				Console.WriteLine("Transaction not found");
			}
			catch (EntityAlreadyExistsException ex)
			{
				Console.WriteLine("Transaction already exists");
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.ToString());
			}
		}
	}
}



