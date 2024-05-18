using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Models;
using Generic.CQRS.Abstractions;

namespace Domain.CQRS.Abstractions;

internal interface IGetTransactionQueryHandler : IGetEntityQuery<Transaction, IGetTransactionParameter, int>
{
}