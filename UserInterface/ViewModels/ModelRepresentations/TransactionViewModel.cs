using System;
using MSkaut;
using UserInterface.Commands;
using DatabaseManager;
using UserManager;
using System.Collections.ObjectModel;
using Supabase;

namespace UserInterface.ViewModels.ModelRepresantations
{
	public class TransactionViewModel : EditableClass
	{
		private Transaction transaction;

        public long? Id { get => transaction.Id; set => transaction.Id = value; }
		public string Name { get => transaction.Name; set => transaction.Name = value; }
        public TransactionType Type { get => transaction.Type; set => transaction.Type = value; }
        public int Amount { get => transaction.Amount; set => transaction.Amount = value; }
        public DateTime Date { get => transaction.Date; set => transaction.Date = value; }

		public TransactionViewModel(Transaction transaction, Client client) : base(client)
		{
            this.transaction = transaction;
		}

        public static async Task<ObservableCollection<TransactionViewModel>> GetEventTransactions(long eventId,
        Dictionary<long, TransactionType> transactionTypes, Client client)
        {
            List<DBTransaction> dbTransactions = await DBTransaction.GetEventTransactions(eventId, client);
            ObservableCollection<TransactionViewModel> result = new();

            foreach (DBTransaction dbTransaction in dbTransactions)
            {
                Transaction transaction = new Transaction(dbTransaction.Id, dbTransaction.Name,
                    dbTransaction.Amount, dbTransaction.Date.ToDateTime(TimeOnly.Parse("10:00 PM")), transactionTypes[dbTransaction.TypeId]);

                result.Add(new TransactionViewModel(transaction, client));
            }

            return result;
        }

        public override void SaveRow(object obj)
        {
            throw new NotImplementedException();
        }

        public override void DeleteRow(object obj)
        {
            throw new NotImplementedException();
        }
    }
}
