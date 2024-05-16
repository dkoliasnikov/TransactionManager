using Domain.Abstractions;
using Domain.Enums;
using Domain.Models;

namespace Domain.Comands.Handlers;

internal class GetTransactionQueryHandler 
	: GetEntityQueryBaseHandler<Transaction, ITransactionRepository>, 
	  IGetTransactionQueryHandler
{
	public GetTransactionQueryHandler(ITransactionRepository transactionRepository, EnityNotFoundBehavior notFoundBehavior) 
		: base(transactionRepository, notFoundBehavior)
	{
	}
}