using Domain.Abstractions;
using Domain.CQRS.Abstractions;
using Domain.CQRS.Abstractions.Params;
using Domain.Enums;
using Domain.Models;

namespace Domain.CQRS.Handlers;

internal class AddOrUpdateTransactionCommandHandler
    : AddOrUpdateEntityCommandBaseHandler<Transaction, ITransactionRepository, AddTransactionParameter, int>, 
    IAddOrUpdateTransactionCommandHandler
{
    public AddOrUpdateTransactionCommandHandler(ITransactionRepository transactionRepository, EntityAlreadyExistsBehavior alreadyExistsBehavior, EnityNotFoundBehavior notFoundBehavior) : base(transactionRepository, alreadyExistsBehavior, notFoundBehavior)
    {
    }
}