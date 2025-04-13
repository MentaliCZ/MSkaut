using System;

public class TransactionType
{
	public string Name { get; private set; }
	private bool isExpense;

	public TransactionType(string name, bool isExpense)
	{
		this.Name = name;
		this.isExpense = isExpense;
	}

	public bool setIsExpense(bool isExpense) 
	{
		this.isExpense = isExpense;
	}
}
