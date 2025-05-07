using System;
using System.Collections.ObjectModel;
using DatabaseManager;
using UserManager;
using Supabase;

namespace MSkaut
{
	public class TransactionType
	{
		public long? Id { get; set; }
		public string Name { get; set; }
		public string Description { get; set; }
		public bool IsExpense { get; set; }

		public TransactionType(long id, string name, string description)
		{
			this.Id = id;
			this.Name = name;
			this.Description = description;
			this.IsExpense = IsExpense;
		}

        public override string ToString()
        {
			return Name;
        }
    }
}