using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Models;
using Generic.CQRS.Abstractions;

namespace Domain.CQRS.Abstractions;

internal interface IAddOrUpdateTransactionCommandHandler : 
	IAddEntityCommand<Transaction, IAddTransactionParameter>
{ 
}