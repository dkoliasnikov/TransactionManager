using Domain.CQRS.Abstractions.Params;
using Domain.Models;

namespace Domain.CQRS.Abstractions;

internal interface IGetTransactionQueryHandler : IGetEntityQuery<Transaction, GetTransactionParameter, int>
{
}