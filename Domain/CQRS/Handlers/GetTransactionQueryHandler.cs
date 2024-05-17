using Domain.Abstractions;
using Domain.CQRS.Abstractions;
using Domain.CQRS.Abstractions.Params;
using Domain.Enums;
using Domain.Models;

namespace Domain.Comands.Handlers;

internal class GetTransactionQueryHandler : GetEntityQueryBaseHandler<Transaction, ITransactionRepository, GetTransactionParameter, int>,
	  IGetTransactionQueryHandler
{
	public GetTransactionQueryHandler(ITransactionRepository transactionRepository, EnityNotFoundBehavior notFoundBehavior) 
		: base(transactionRepository, notFoundBehavior)
	{
	}
}