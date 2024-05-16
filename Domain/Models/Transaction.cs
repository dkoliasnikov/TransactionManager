using Domain.Abstractions;

namespace Domain.Models;

public class Transaction : IHaveId
{
	public int Id { get; set; }
	public DateTime TransactionDate { get; set; }
	public decimal Amount { get; set; }
}
