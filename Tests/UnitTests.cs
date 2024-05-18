using Autofac;
using Domain;
using Domain.Abstractions;
using Domain.CQRS.Abstractions;
using Domain.CQRS.Abstractions.Params;
using Domain.Models;
using FluentAssertions;
using Generic.Exceptions;
using InMemoryStorage;
using static System.Formats.Asn1.AsnWriter;

namespace Tests;

public class UnitTests
{
	[Fact]
	public async void Add_Transaction_Successfully()
	{ 
		// Arrange
		var container = ConfigureServices();
		var addHandler = container.Resolve<IAddOrUpdateTransactionCommand>();
		var repository = container.Resolve<ITransactionRepository>();
		var addingTransaction = new Transaction(0, DateTime.Parse("15.01.2024"), 100500);

		// Act
		await addHandler.AddOrUpdateAsync(addingTransaction);
		var transaction = await repository.GetAsync(addingTransaction.Id);

		// Assert
		transaction.Should().NotBeNull();
		transaction.Id.Should().Be(addingTransaction.Id);
		transaction.TransactionDate.Should().Be(addingTransaction.TransactionDate);
		transaction.Amount.Should().Be(addingTransaction.Amount);
	}

	[Fact]
	public async void On_Duplicated_Transaction_Throw()
	{
		// Arrange
		var container = ConfigureServices();
		var addHandler = container.Resolve<IAddOrUpdateTransactionCommand>();
		var repository = container.Resolve<ITransactionRepository>();
		var addingTransaction = new Transaction(0, DateTime.Parse("15.01.2024"), 100500);
		await repository.AddAsync(addingTransaction);
		var act = async () => await addHandler.AddOrUpdateAsync(addingTransaction);

		// Act & Assert
		await act.Should().ThrowAsync<EntityAlreadyExistsException>();	
	}


	[Fact]
	public async void Get_Transaction_Successfully()
	{
		// Arrange
		var container = ConfigureServices();
		var transactionQuery = container.Resolve<ITransactionQuery>();
		var repository = container.Resolve<ITransactionRepository>();
		var addingTransaction = new Transaction(0, DateTime.Parse("15.01.2024"), 100500);
		await repository.AddAsync(addingTransaction);
		var queryTransactionParameter = new QueryTransactionParameter(addingTransaction.Id);

		// Act
		var addedTransaction = await transactionQuery.GetAsync(queryTransactionParameter);

		// Assert
		addedTransaction.Should().NotBeNull();
		addedTransaction.Id.Should().Be(addingTransaction.Id);
		addedTransaction.TransactionDate.Should().Be(addingTransaction.TransactionDate);
		addedTransaction.Amount.Should().Be(addingTransaction.Amount);
	}

	[Fact]
	public async void On_Query_Not_Existing_Transaction_Throw()
	{
		// Arrange
		var container = ConfigureServices();
		var transactionQuery = container.Resolve<ITransactionQuery>();
		var act = async () => await transactionQuery.GetAsync(new QueryTransactionParameter(0));

		// Act & Assert
		await act.Should().ThrowAsync<EntityNotFoundException>();
	}

	private static IContainer ConfigureServices() => new ContainerBuilder().AddDomain().AddInMemoryStorage().Build();
}