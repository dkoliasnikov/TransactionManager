using Domain.Abstractions;
using Domain.Models;
using Generic.Abstractions;
using InMemoryStorage.Abstractions;

namespace InMemoryStorage.Repositories;

internal class InMemoryTransactionRepository : BaseRepository<Transaction>, IRepository<Transaction, int>, ITransactionRepository
{

}