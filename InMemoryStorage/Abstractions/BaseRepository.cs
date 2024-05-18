using System.Collections.Concurrent;
using Mapster;
using Generic.Abstractions;
using Generic.Exceptions;

namespace InMemoryStorage.Abstractions;

internal class BaseRepository<EntityT, KeyT>
	where EntityT : IHaveKey<KeyT>
{
	private readonly ConcurrentDictionary<KeyT, EntityT> _storage = new();

	public Task AddAsync(EntityT entity)
	{
		if (!_storage.TryAdd(entity.Id, entity))
			throw new EntityAlreadyExistsException($"Transaction with id {entity.Id} already exists");

		return Task.CompletedTask;
	}

	public Task<EntityT> GetAsync(KeyT id)
	{
		if (!_storage.TryGetValue(id, out var transaction))
			throw new EntityNotFoundException($"Transaction with id {id} not found");

		return Task.FromResult(transaction);
	}

	public async Task UpdateAsync(EntityT entity)
	{
		var updatingEnitity = await GetAsync(entity.Id);

		entity.Adapt(updatingEnitity);

		_storage.AddOrUpdate(entity.Id, entity, (key, oldValue) => entity);
	}
}