using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Models;
using Generic.CQRS.Abstractions;

namespace Domain.CQRS.Abstractions;

public interface IAddOrUpdateTransactionCommandHandler : 
	IAddEntityCommand<Transaction, IAddTransactionParameter>
{ 
}