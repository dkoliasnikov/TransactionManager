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
	private readonly Dictionary<Type, Func<object, Task>> _handlersFactory ;
	private Dictionary<string, ParameterFactory> _parametersFactory;
	private readonly ILifetimeScope _scope;
	private readonly IInputFetcher _inputFetcher;
	private readonly IOutputPrinter _outputPrinter;
	private delegate bool ParserDelegate<T>(string input, out T output);
	private record ParameterFactory(Func<IParameter> ParameterBuilder, Type CommandType);

	public TransactionManager(ILifetimeScope scope, IInputFetcher userInputFetcher, IOutputPrinter outputPrinter)
	{
		_scope = scope;
		_inputFetcher = userInputFetcher;
		_outputPrinter = outputPrinter;

		_handlersFactory = new()
			{
				{
					typeof(IGetTransactionQueryHandler), 
					async (parameter) => outputPrinter.WriteLine(JsonSerializer.Serialize(await _scope.Resolve<IGetTransactionQueryHandler>().GetAsync(parameter as IGetTransactionParameter)))
				},
				{
					typeof(IAddOrUpdateTransactionCommandHandler), 
					(parameter) => _scope.Resolve<IAddOrUpdateTransactionCommandHandler>().Handle(parameter as IAddTransactionParameter)
				},
				{
					typeof(IExitCommand), 
					(parameter) => _scope.Resolve<IExitCommand>().Handle(parameter as ExitAppParameter)
				}
			};

		_parametersFactory = new Dictionary<string, ParameterFactory>()
				{
					{ "exit",  new (() => new ExitAppParameter(), typeof(IExitCommand)) },
					{ "get",   new (() => new GetTransactionParameter(FetchValue<int>("Введите id", int.TryParse)), typeof(IGetTransactionQueryHandler))},
					{ "add",  new (() => new AddTransactionParameter(new Transaction(
						FetchValue<int>("Введите id", int.TryParse),
						FetchValue<DateTime>("Введите дату", DateTime.TryParse),
						FetchValue<int>("Введите сумму", int.TryParse))), typeof(IAddOrUpdateTransactionCommandHandler))
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

				if(_parametersFactory.TryGetValue(_inputFetcher.FetchNext().Trim().ToLower(), out var builderWithCommand))
				{
					await _handlersFactory[builderWithCommand.CommandType].Invoke(builderWithCommand.ParameterBuilder());
					_outputPrinter.WriteLine("[Ok]");
				}
				else
				{
					_outputPrinter.WriteLine("Неизвестная команда");
				}
			}
			catch (EntityNotFoundException ex)
			{
				_outputPrinter.WriteLine("Транзакция не найдена");
			}
			catch (EntityAlreadyExistsException ex)
			{
				_outputPrinter.WriteLine("Транзакция уже существует");
			}
			catch (Exception ex)
			{
				_outputPrinter.WriteLine(ex.ToString());
			}
		}
	}
	
	private T FetchValue<T>(string tag, ParserDelegate<T> parser )
	{
		_outputPrinter.Write($"{tag} ");
		T value;
		while (!parser.Invoke(_inputFetcher.FetchNext(), out value))
		{
			_outputPrinter.WriteLine("Некорректное значение");
		}
		
		return value;
	}
}