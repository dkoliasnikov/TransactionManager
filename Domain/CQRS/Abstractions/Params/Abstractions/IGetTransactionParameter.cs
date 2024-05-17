using Domain.Models;

namespace Domain.CQRS.Abstractions.Params.Abstractions;

internal interface IGetTransactionParameter : IGetEntityParameter<Transaction, int>
{
}