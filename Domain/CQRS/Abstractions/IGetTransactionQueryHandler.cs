using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Models;

namespace Domain.CQRS.Abstractions;

internal interface IGetTransactionQueryHandler : IGetEntityQuery<Transaction, IGetTransactionParameter, int>
{
}