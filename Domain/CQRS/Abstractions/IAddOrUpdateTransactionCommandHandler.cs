using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Models;

namespace Domain.CQRS.Abstractions;

internal interface IAddOrUpdateTransactionCommandHandler : 
	IAddEntityCommand<Transaction, IAddTransactionParameter>
{ 
}