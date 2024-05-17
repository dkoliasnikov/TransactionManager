using Domain.CQRS.Abstractions.Params;
using Domain.Models;

namespace Domain.CQRS.Abstractions;

internal interface IAddOrUpdateTransactionCommandHandler : 
	IAddEntityCommand<Transaction, AddTransactionParameter>
{ 
}