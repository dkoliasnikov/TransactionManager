namespace Domain.CQRS.Abstractions;

internal interface IGetEntityQuery<EntityT> : IUserQuery
{
    /// <exception cref="EntityNotFoundException"></exception>
    Task<EntityT?> GetAsync(int id);
}