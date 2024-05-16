namespace Domain.CQRS.Abstractions;

internal interface IAddEntityCommand<EntityT> : IUserCommand
{
    /// <exception cref="EntityNotFoundException"></exception>
    Task AddOrUpdateAsync(EntityT transaction);
}