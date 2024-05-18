using Domain.Abstractions;
using Domain.CQRS.Abstractions;
using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Models;
using Generic.CQRS.Abstractions;
using Generic.Enums;

namespace Domain.CQRS.Handlers;

internal class GetTransactionQueryHandler : GetEntityQueryBaseHandler<Transaction, ITransactionRepository, IQueryTransactionParameter, int>,
	  ITransactionQuery
{
	public GetTransactionQueryHandler(ITransactionRepository transactionRepository, EnityNotFoundBehavior notFoundBehavior)
		: base(transactionRepository, notFoundBehavior)
	{
	}
}