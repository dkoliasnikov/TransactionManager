using Domain.Abstractions;
using Domain.CQRS.Abstractions;
using Domain.CQRS.Abstractions.Params.Abstractions;
using Domain.Models;
using Generic.CQRS.Abstractions;
using Generic.Enums;

namespace Domain.CQRS.Handlers;

internal class AddOrUpdateTransactionCommandHandler
    : AddOrUpdateEntityCommandBaseHandler<Transaction, ITransactionRepository, IAddTransactionParameter, int>, 
    IAddOrUpdateTransactionCommandHandler
{
    public AddOrUpdateTransactionCommandHandler(ITransactionRepository transactionRepository, EntityAlreadyExistsBehavior alreadyExistsBehavior, EnityNotFoundBehavior notFoundBehavior) : base(transactionRepository, alreadyExistsBehavior, notFoundBehavior)
    {
    }
}