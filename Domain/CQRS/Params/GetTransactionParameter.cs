using Domain.CQRS.Abstractions.Params.Abstractions;

namespace Domain.CQRS.Abstractions.Params;

internal class GetTransactionParameter : IGetTransactionParameter
{
	public GetTransactionParameter(int key)
	{
		Key = key;
	}

	public int Key { get; set; }
}