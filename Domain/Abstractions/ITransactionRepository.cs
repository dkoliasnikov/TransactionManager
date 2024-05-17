using Domain.Models;

namespace Domain.Abstractions;

public interface ITransactionRepository : IRepository<Transaction, int> { }