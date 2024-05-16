using Domain.Exceptions;
using System.Collections.Concurrent;
using Mapster;
using Domain.Abstractions;

namespace InMemoryStorage.Abstracations;

internal class BaseRepository <EntityT> 
	where EntityT : IHaveId
{
	private readonly ConcurrentDictionary<int, EntityT> _storage = new();

	public Task AddAsync(EntityT entity)
	{
		if (!_storage.TryAdd(entity.Id, entity))
			throw new EntityAlreadyExistsException($"Transaction with id {entity.Id} already exists");

		return Task.CompletedTask;
	}

	public Task<EntityT> GetAsync(int id)
	{
		if (!_storage.TryGetValue(id, out var transaction))
			throw new EntityNotFoundException($"Transaction with id {id} not found");

		return Task.FromResult(transaction);
	}

	public Task UpdateAsync(EntityT entity)
	{
		var updatingEnitity = GetAsync(entity.Id);

		entity.Adapt(updatingEnitity);

		return Task.CompletedTask;
	}
}