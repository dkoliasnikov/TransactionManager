using Domain.CQRS.Abstractions.Params.Abstractions;

namespace Domain.CQRS.Abstractions.Params;

public class QueryTransactionParameter : IQueryTransactionParameter
{
	public QueryTransactionParameter(int key)
	{
		Key = key;
	}

	public int Key { get; set; }
}