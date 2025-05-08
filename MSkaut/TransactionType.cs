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
		public long? OwnerId { get; set; }

		public TransactionType(string name, string description, long? ownerId)
		{
			this.Id = null;
			this.Name = name;
			this.Description = description;
			this.OwnerId = ownerId;
		}

        public TransactionType(long id, string name, string description, long? ownerId) : this(name, description, ownerId)
        {
            this.Id = id;
        }

        public override string ToString()
        {
			return Name;
        }
    }
}