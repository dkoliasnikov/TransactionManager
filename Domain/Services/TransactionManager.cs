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
	private Dictionary<string, CommandFactory> _commandsFactory;
	private readonly ILifetimeScope _scope;
	private readonly IInputFetcher _inputFetcher;
	private readonly IOutputPrinter _outputPrinter;
	private delegate bool Parser<T>(string input, out T output);
	private record CommandFactory(Func<IParameter> ParameterBuilder, Func<IParameter, Task> CommandBuilder);
	private readonly static string _transactionNotFoundMessage = "Транзакция не найдена";
	private readonly static string _transactionAlreadyExistsMessage = "Транзакция уже существует";
	private readonly static string _exitingAppMessage = "Завершаем работу программы";
	private readonly static string _unknownCommandMessage = "Неизвестная команда";
	private readonly static string _inputCommandMessage = "Введите команду ";
	private readonly static string _inputIdMessage = "Введите id";
	private readonly static string _inputDateMessage = "Введите дату";
	private readonly static string _inputAmountMessage = "Введите дату";
	private readonly static string _okMessage = "[Ok]";
	private readonly static string _incorrectInputMessage = "Некорректное значение";
	
	public TransactionManager(ILifetimeScope scope, IInputFetcher userInputFetcher, IOutputPrinter outputPrinter)
	{
		_scope = scope;
		_inputFetcher = userInputFetcher;
		_outputPrinter = outputPrinter;

		_commandsFactory = new Dictionary<string, CommandFactory>()
				{
					{ "exit",
						new (() => new ExitAppParameter(),
						(parameter) => _scope.Resolve<IExitCommand>().Handle(parameter as ExitAppParameter)
						) },
					{ "get",
						new (() => new QueryTransactionParameter(FetchValue<int>("Введите id", int.TryParse)),
						async (parameter) => {
							var transaction = await _scope.Resolve<ITransactionQuery>().GetAsync(parameter as QueryTransactionParameter);
							outputPrinter.WriteLine(transaction is not null ? JsonSerializer.Serialize(transaction) : _transactionNotFoundMessage);
						})
					},
					{ "add",  
						new (() => new AddTransactionParameter(new Transaction(
							FetchValue<int>(_inputIdMessage, int.TryParse),
							FetchValue<DateTime>(_inputDateMessage, DateTime.TryParse),
							FetchValue<int>(_inputAmountMessage, int.TryParse))),
						(parameter) => _scope.Resolve<IAddOrUpdateTransactionCommand>().Handle(parameter as AddTransactionParameter))
					}
				};
	}

	public async Task Run()
	{
		while (true)
		{
			try
			{
				_outputPrinter.WriteLine(_inputCommandMessage);

				if(_commandsFactory.TryGetValue(_inputFetcher.FetchNext().Trim().ToLower(), out var commandCreator))
				{
					await commandCreator.CommandBuilder.Invoke(commandCreator.ParameterBuilder());
					_outputPrinter.WriteLine(_okMessage);
				}
				else
				{
					_outputPrinter.WriteLine(_unknownCommandMessage);
				}
			}
			catch (EntityNotFoundException)
			{
				_outputPrinter.WriteLine(_transactionNotFoundMessage);
			}
			catch (EntityAlreadyExistsException)
			{
				_outputPrinter.WriteLine(_transactionAlreadyExistsMessage);
			}
			catch(TerminatedByUserException)
			{
				_outputPrinter.WriteLine(_exitingAppMessage);
				break;
			}
			catch (Exception ex)
			{
				_outputPrinter.WriteLine(ex.ToString());
			}
		}
	}
	
	private T FetchValue<T>(string tag, Parser<T> parser )
	{
		T value;
		do
		{
			_outputPrinter.Write($"{tag} ");
			if (parser.Invoke(_inputFetcher.FetchNext(), out value))
				break;
			else
				_outputPrinter.WriteLine(_incorrectInputMessage);
		} while (true);

		return value;
	}
}