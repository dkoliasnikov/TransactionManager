using Generic.Abstractions;

namespace Domain.Models;

public class Transaction : IHaveId
{
	public Transaction(int id, DateTime transactionDate, decimal amount)
	{
		Id = id;
		TransactionDate = transactionDate;
		Amount = amount;
	}

	public int Id { get; set; }
	public DateTime TransactionDate { get; set; }
	public decimal Amount { get; set; }
}
