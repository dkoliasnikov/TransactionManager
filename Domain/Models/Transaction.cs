using Generic.Abstractions;

namespace Domain.Models;

public record Transaction(int Id, DateTime TransactionDate, decimal Amount) : IHaveId;
