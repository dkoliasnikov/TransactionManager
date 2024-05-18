using Autofac;
using Domain.Abstractions;
using Domain.CQRS.Abstractions;
using Domain.CQRS.Abstractions.Params;
using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Exceptions;
using Domain.Models;
using Generic.CQRS.Abstractions.Params.Abstractions;
using Generic.Exceptions;
using System.Text.Json;

namespace Domain.Services;

internal class TransactionManager : ITransactionManager
{
	private Dictionary<string, CommandFactory> _handlersFactory;
	private readonly ILifetimeScope _scope;
	private readonly IInputFetcher _inputFetcher;
	private readonly IOutputPrinter _outputPrinter;
	private delegate bool ParserDelegate<T>(string input, out T output);
	private record CommandFactory(Func<IParameter> ParameterBuilder, Func<object, Task> CommandBuilder);

	public TransactionManager(ILifetimeScope scope, IInputFetcher userInputFetcher, IOutputPrinter outputPrinter)
	{
		_scope = scope;
		_inputFetcher = userInputFetcher;
		_outputPrinter = outputPrinter;

		_handlersFactory = new Dictionary<string, CommandFactory>()
				{
					{ "exit",  
						new (() => new ExitAppParameter(),
						(parameter) => _scope.Resolve<IExitCommand>().Handle(parameter as ExitAppParameter)
						) },
					{ "get",   
						new (() => new QueryTransactionParameter(FetchValue<int>("Введите id", int.TryParse)), 
						async (parameter) => outputPrinter.WriteLine(JsonSerializer.Serialize(await _scope.Resolve<ITransactionQuery>().GetAsync(parameter as IQueryTransactionParameter))))
					},
					{ "add",  
						new (() => new AddTransactionParameter(new Transaction(
							FetchValue<int>("Введите id", int.TryParse),
							FetchValue<DateTime>("Введите дату", DateTime.TryParse),
							FetchValue<int>("Введите сумму", int.TryParse))),
						(parameter) => _scope.Resolve<IAddOrUpdateTransactionCommand>().Handle(parameter as IAddTransactionParameter))
					}
				};
	}

	public async Task Run()
	{
		while (true)
		{
			try
			{
				_outputPrinter.WriteLine("Введите команду ");

				if(_handlersFactory.TryGetValue(_inputFetcher.FetchNext().Trim().ToLower(), out var builderWithCommand))
				{
					await builderWithCommand.CommandBuilder.Invoke(builderWithCommand.ParameterBuilder());
					_outputPrinter.WriteLine("[Ok]");
				}
				else
				{
					_outputPrinter.WriteLine("Неизвестная команда");
				}
			}
			catch (EntityNotFoundException)
			{
				_outputPrinter.WriteLine("Транзакция не найдена");
			}
			catch (EntityAlreadyExistsException)
			{
				_outputPrinter.WriteLine("Транзакция уже существует");
			}
			catch(TerminatedByUserException)
			{
				_outputPrinter.WriteLine("Завершаем работу программы");
				break;
			}
			catch (Exception ex)
			{
				_outputPrinter.WriteLine(ex.ToString());
			}
		}
	}
	
	private T FetchValue<T>(string tag, ParserDelegate<T> parser )
	{
		T value;
		do
		{
			_outputPrinter.Write($"{tag} ");
			if (parser.Invoke(_inputFetcher.FetchNext(), out value))
				break;
			else
				_outputPrinter.WriteLine("Некорректное значение");
		} while (true);

		return value;
	}
}