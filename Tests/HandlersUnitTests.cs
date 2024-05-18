using Autofac;
using Domain;
using Domain.Abstractions;
using Domain.CQRS.Abstractions;
using Domain.CQRS.Abstractions.Params;
using Domain.Models;
using FluentAssertions;
using Generic.Enums;
using Generic.Exceptions;
using InMemoryStorage;

namespace Tests;

public class HandlersUnitTests
{
	[Fact]
	public async void Add_Transaction_Successfully()
	{ 
		// Arrange
		var container = ConfigureServices(EntityAlreadyExistsBehavior.PropagateException, EnityNotFoundBehavior.PropagateException);
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
	public async void Adding_Existing_Transaction_Throw()
	{
		// Arrange
		var container = ConfigureServices(EntityAlreadyExistsBehavior.PropagateException, EnityNotFoundBehavior.PropagateException);
		var addHandler = container.Resolve<IAddOrUpdateTransactionCommand>();
		var repository = container.Resolve<ITransactionRepository>();
		var addingTransaction = new Transaction(0, DateTime.Parse("15.01.2024"), 100500);
		await repository.AddAsync(addingTransaction);
		var act = async () => await addHandler.AddOrUpdateAsync(addingTransaction);

		// Act & Assert
		await act.Should().ThrowAsync<EntityAlreadyExistsException>();	
	}

	[Fact]
	public async void Adding_Existing_Transaction_Ignore()
	{
		// Arrange
		var container = ConfigureServices(EntityAlreadyExistsBehavior.Ignore, EnityNotFoundBehavior.PropagateException);
		var addHandler = container.Resolve<IAddOrUpdateTransactionCommand>();
		var repository = container.Resolve<ITransactionRepository>();
		var addingTransaction = new Transaction(0, DateTime.Parse("15.01.2024"), 100500);
		await repository.AddAsync(addingTransaction);
		var act = async () => await addHandler.AddOrUpdateAsync(addingTransaction);

		// Act & Assert
		await act.Should().NotThrowAsync<EntityAlreadyExistsException>();
	}

	[Fact]
	public async void Adding_Existing_Transaction_Update_Existing()
	{
		// Arrange
		var container = ConfigureServices(EntityAlreadyExistsBehavior.Update, EnityNotFoundBehavior.PropagateException);
		var addHandler = container.Resolve<IAddOrUpdateTransactionCommand>();
		var repository = container.Resolve<ITransactionRepository>();
		var transaction = new Transaction(0, DateTime.Parse("15.01.2024"), 100500);
		var duplicatedTransaction = new Transaction(0, DateTime.Parse("11.02.2023"), 55555);
		await repository.AddAsync(transaction);

		await addHandler.AddOrUpdateAsync(duplicatedTransaction);
		var updatedTransaction = await repository.GetAsync(duplicatedTransaction.Id);

		// Act & Assert
		updatedTransaction.Should().NotBeNull();
		updatedTransaction.Id.Should().Be(duplicatedTransaction.Id);
		updatedTransaction.TransactionDate.Should().Be(duplicatedTransaction.TransactionDate);
		updatedTransaction.Amount.Should().Be(duplicatedTransaction.Amount);
	}

	[Fact]
	public async void Query_Transaction_Successfully()
	{
		// Arrange
		var container = ConfigureServices(EntityAlreadyExistsBehavior.PropagateException, EnityNotFoundBehavior.PropagateException);
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
	public async void Query_Not_Existing_Transaction_Throw()
	{
		// Arrange
		var container = ConfigureServices(EntityAlreadyExistsBehavior.PropagateException, EnityNotFoundBehavior.PropagateException);
		var transactionQuery = container.Resolve<ITransactionQuery>();
		var act = async () => await transactionQuery.GetAsync(new QueryTransactionParameter(0));

		// Act & Assert
		await act.Should().ThrowAsync<EntityNotFoundException>();
	}

	[Fact]
	public async void Query_Not_Existing_Transaction_Ignore()
	{
		// Arrange
		var container = ConfigureServices(EntityAlreadyExistsBehavior.PropagateException, EnityNotFoundBehavior.Ignore);
		var transactionQuery = container.Resolve<ITransactionQuery>();

		// Act
		var transaction = await transactionQuery.GetAsync(new QueryTransactionParameter(0));

		// Assert
		transaction.Should().BeNull();
	}

	private static IContainer ConfigureServices(EntityAlreadyExistsBehavior alreadyExistsBehavior, EnityNotFoundBehavior notFoundBehavior) => 
		new ContainerBuilder().AddDomain(alreadyExistsBehavior, notFoundBehavior).AddInMemoryStorage().Build();
}