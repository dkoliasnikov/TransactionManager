using Domain.Exceptions;
using Domain.Models;
using System.Collections.Concurrent;
using Mapster;

namespace InMemoryStorage.Abstracations;

internal class BaseRepository
{
	private readonly ConcurrentDictionary<int, Transaction> _storage = new();

	public Task AddAsync(Transaction entity)
	{
		if (!_storage.TryAdd(entity.Id, entity))
			throw new EntityAlreadyExistsException($"Transaction with id {entity.Id} already exists");

		return Task.CompletedTask;
	}

	public Task<Transaction> GetAsync(int id)
	{
		if (!_storage.TryGetValue(id, out var transaction))
			throw new EntityNotFoundException($"Transaction with id {id} not found");

		return Task.FromResult(transaction);
	}

	public Task UpdateAsync(Transaction entity)
	{
		var updatingEnitity = GetAsync(entity.Id);

		entity.Adapt(updatingEnitity);

		return Task.CompletedTask;
	}
}