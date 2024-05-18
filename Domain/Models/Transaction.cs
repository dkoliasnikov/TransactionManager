using Generic.Abstractions;

namespace Domain.Models;

public class Transaction : IHaveKey<int>
{
	public Transaction(int id, DateTime transactionDate, int amount)
	{
		Id = id;
		TransactionDate = transactionDate;
		Amount = amount;
	}

	public int Id { get; set; }
    
    public DateTime TransactionDate { get; set; }
   
    public int Amount { get; set; }
}
