
using Domain.Models;
using Generic.CQRS.Abstractions.Params.Abstractions;

namespace Domain.CQRS.Abstractions.Params.Abstractions;

public interface IAddTransactionParameter : IAddEntityParameter<Transaction>
{
	
}