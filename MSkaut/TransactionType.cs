using System;

namespace MSkaut
{
	public class TransactionType
	{
		public string Name { get; private set; }
		private bool isExpense;

		public TransactionType(string name, bool isExpense)
		{
			this.Name = name;
			this.isExpense = isExpense;
		}

		public void setIsExpense(bool isExpense)
		{
			this.isExpense = isExpense;
		}
	}
}