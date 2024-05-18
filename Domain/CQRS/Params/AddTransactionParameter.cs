using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Models;

namespace Domain.CQRS.Abstractions.Params;

public class AddTransactionParameter : IAddTransactionParameter
{
	public AddTransactionParameter(Transaction transaction)
	{
		Transaction = transaction;
	}

	public Transaction Transaction { get; set; }
}