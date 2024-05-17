using Domain.Models;
using Generic.Abstractions;

namespace Domain.Abstractions;

public interface ITransactionRepository : IRepository<Transaction, int> { }