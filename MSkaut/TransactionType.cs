using System;

namespace MSkaut
{
	public class TransactionType
	{
		public string Name { get; private set; }
		public string Description { get; private set; }

		public TransactionType(string name, string description)
		{
			this.Name = name;
			this.Description = description;
		}

	}
}