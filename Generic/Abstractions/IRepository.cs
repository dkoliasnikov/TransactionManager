namespace Generic.Abstractions;

public interface IRepository<EntityT, KeyT>
{
	/// <exception cref="EntityAlreadyExistsException"></exception>
	Task AddAsync(EntityT entity);

	/// <exception cref="EntityNotFoundException"></exception>
	Task<EntityT> GetAsync(KeyT id);

	/// <exception cref="EntityNotFoundException"></exception>
	Task UpdateAsync(EntityT entity);
}