using Autofac;
using Domain;
using Domain.Abstractions;
using Domain.Models;
using FluentAssertions;
using Generic.Enums;
using InMemoryStorage;
using System.Text.Json;
using Tests.Helpers;

namespace Tests;

public class TransactionManagerUnitTests
{
	[Fact]
	public async void Add_Transaction_Successfully()
	{
		// Arrange
		var container = ConfigureServices(EntityAlreadyExistsBehavior.PropagateException, EnityNotFoundBehavior.PropagateException);
		var repository = container.Resolve<ITransactionRepository>();
		var transactionManager = container.Resolve<ITransactionManager>();
		var inputFetcher = container.Resolve<IInputFetcher>() as MockInputFetcher;

		var addingTransaction = new Transaction(0, DateTime.Parse("15.01.2024"), 100500);

		inputFetcher!.Values = new string[]{ "add", addingTransaction.Id.ToString(), addingTransaction.TransactionDate.ToString("dd.MM.yyyy"), addingTransaction.Amount.ToString() , "exit"};

		// Act
		await transactionManager.Run();
		var transaction = await repository.GetAsync(addingTransaction.Id);

		// Assert
		transaction.Should().NotBeNull();
		transaction.Id.Should().Be(addingTransaction.Id);
		transaction.TransactionDate.Should().Be(addingTransaction.TransactionDate);
		transaction.Amount.Should().Be(addingTransaction.Amount);
	}

	[Fact]
	public async void Get_Transaction_Successfully()
	{
		// Arrange
		var container = ConfigureServices(EntityAlreadyExistsBehavior.PropagateException, EnityNotFoundBehavior.PropagateException);
		var repository = container.Resolve<ITransactionRepository>();
		var transactionManager = container.Resolve<ITransactionManager>();
		var inputFetcher = container.Resolve<IInputFetcher>() as MockInputFetcher;
		var outputFetcher = container.Resolve<IOutputPrinter>() as MockOutputPrinter;

		var addingTransaction = new Transaction(0, DateTime.Parse("15.01.2024"), 100500);

		await repository.AddAsync(addingTransaction);

		inputFetcher!.Values = new string[] { "get", addingTransaction.Id.ToString(), "exit" };

		// Act
		await transactionManager.Run();

		// Assert
		outputFetcher.Values.Count().Should().Be(5);
		var deserializedTransAction = JsonSerializer.Deserialize<Transaction>(outputFetcher.Values[1]);

		deserializedTransAction.Id.Should().Be(addingTransaction.Id);
		deserializedTransAction.TransactionDate.Should().Be(addingTransaction.TransactionDate);
		deserializedTransAction.Amount.Should().Be(addingTransaction.Amount);
	}

	private static IContainer ConfigureServices(EntityAlreadyExistsBehavior alreadyExistsBehavior, EnityNotFoundBehavior notFoundBehavior)
	{
		var builder = new ContainerBuilder().AddDomain(alreadyExistsBehavior, notFoundBehavior).AddInMemoryStorage();
		builder.RegisterType<MockInputFetcher>().As<IInputFetcher>().SingleInstance();
		builder.RegisterType<MockOutputPrinter>().As<IOutputPrinter>().SingleInstance();

		return builder.Build();
	}
}