namespace Domain.Comands;

internal interface IGetEntityQuery<EntityT> : CQRS.Abstractions.IUserQuery
{
	/// <exception cref="EntityNotFoundException"></exception>
	Task<EntityT?> GetAsync(int id);
}