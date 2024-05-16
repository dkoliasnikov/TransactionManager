namespace Domain.Abstractions;

public interface IRepository<EntityT>
{
	/// <exception cref="EntityAlreadyExistsException"></exception>
	Task AddAsync(EntityT entity);

	/// <exception cref="EntityNotFoundException"></exception>
	Task<EntityT> GetAsync(int id);

	/// <exception cref="EntityNotFoundException"></exception>
	Task UpdateAsync(EntityT entity);
}