using System;
using System.Collections.ObjectModel;
using DatabaseManager;
using UserManager;
using Supabase;

namespace MSkaut
{
	public class TransactionType
	{
		public long Id { get; private set; }
		public string Name { get; private set; }
		public string Description { get; private set; }

		public TransactionType(long id, string name, string description)
		{
			this.Id = id;
			this.Name = name;
			this.Description = description;
		}

		public static async Task<ObservableCollection<TransactionType>> GetUsersTransactionTypes(User user, Client client)
		{
			List<DBTransactionType> dbTransactionTypes = await DBTransactionType.GetUsersTransactionTypes(user.Id, client);
			ObservableCollection<TransactionType> result = new();

			foreach (DBTransactionType dbTransactionType in dbTransactionTypes) 
			{
				result.Add(new TransactionType(dbTransactionType.Id, dbTransactionType.Name, dbTransactionType.Description));
			}

			return result;
		}
	}
}