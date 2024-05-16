using Domain.Abstractions;
using Domain.Enums;
using Domain.Models;

namespace Domain.Comands.Handlers;

internal class AddOrUpdateTransactionCommandHandler 
	: AddOrUpdateEntityCommandBaseHandler<Transaction, ITransactionRepository>, IAddOrUpdateTransactionCommandHandler
{
	public AddOrUpdateTransactionCommandHandler(ITransactionRepository transactionRepository, EntityAlreadyExistsBehavior alreadyExistsBehavior, EnityNotFoundBehavior notFoundBehavior) : base(transactionRepository, alreadyExistsBehavior, notFoundBehavior)
	{
	}
}