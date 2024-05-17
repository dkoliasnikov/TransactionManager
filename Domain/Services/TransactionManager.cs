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

	private readonly IInputFetcher _inputFetcher;
	private readonly IOutputPrinter _outputPrinter;

	public TransactionManager(ILifetimeScope scope, IInputFetcher userInputFetcher, IOutputPrinter outputPrinter)
	{
		_scope = scope;
		_inputFetcher = userInputFetcher;

		_handlersMap = new()
			{
				{
					typeof(IGetTransactionQueryHandler), async (parameter) => outputPrinter.WriteLine(JsonSerializer.Serialize(await _scope.Resolve<IGetTransactionQueryHandler>().GetAsync(parameter as IGetTransactionParameter)))
				},
				{
					typeof(IAddOrUpdateTransactionCommandHandler), (parameter) => _scope.Resolve<IAddOrUpdateTransactionCommandHandler>().Handle(parameter as IAddTransactionParameter)

				},
				{
					typeof(IExitCommand), (parameter) => _scope.Resolve<IExitCommand>().Handle(parameter as ExitAppParameter)
				}
			};
		_outputPrinter = outputPrinter;
	}


	public async Task Run()
	{
		while (true)
		{
			try
			{
				_outputPrinter.WriteLine("Введите команду ");
				var command = _inputFetcher.FetchNext();
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
							_outputPrinter.Write("Введите id ");
							var _successfullyParsedId = int.TryParse(_inputFetcher.FetchNext(), out var _id);
							if (!_successfullyParsedId)
							{
								_outputPrinter.WriteLine("Некорректное значение");
								continue;
							}

							request = typeof(IGetTransactionQueryHandler);
							parameter = new GetTransactionParameter(_id);

							break;
						}

						break;

					case "add":
						_outputPrinter.Write("Введите id ");
						var successfullyParsedId = int.TryParse(_inputFetcher.FetchNext(), out var id);
						if (!successfullyParsedId)
						{
							_outputPrinter.WriteLine("Некорректное значение");
							continue;
						}

						_outputPrinter.Write("Введите дату ");
						var successfullyParsedDate = DateTime.TryParse(_inputFetcher.FetchNext(), out var dateTime);
						if (!successfullyParsedDate)
						{
							_outputPrinter.WriteLine("Некорректное значение");
							continue;
						}

						_outputPrinter.Write("Введите сумму ");
						var successfullyParsedAmount = int.TryParse(_inputFetcher.FetchNext(), out var amount);
						if (!successfullyParsedAmount)
						{
							_outputPrinter.WriteLine("Некорректное значение");
							continue;
						}

						request = typeof(IAddOrUpdateTransactionCommandHandler);
						parameter = new AddTransactionParameter(new Transaction(id, dateTime, amount));
						break;
					default:
						_outputPrinter.WriteLine("Неизвестная команда");
						break;
				}

				if (request is not null && parameter is not null)
				{ 
					await _handlersMap[request].Invoke(parameter);
					request = null;
					parameter = null;
					_outputPrinter.WriteLine("[Ok]");
				}

			}
			catch (EntityNotFoundException ex)
			{
				_outputPrinter.WriteLine("Transaction not found");
			}
			catch (EntityAlreadyExistsException ex)
			{
				_outputPrinter.WriteLine("Transaction already exists");
			}
			catch (Exception ex)
			{
				_outputPrinter.WriteLine(ex.ToString());
			}
		}
	}
}



