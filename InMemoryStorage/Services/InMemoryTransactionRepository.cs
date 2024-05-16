using Domain.Abstractions;
using Domain.Models;
using InMemoryStorage.Abstracations;

namespace InMemoryStorage.Services;

internal class InMemoryTransactionRepository : BaseRepository, IRepository<Transaction>
{
	
}