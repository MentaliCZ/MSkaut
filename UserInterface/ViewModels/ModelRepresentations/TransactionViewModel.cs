using System;
using MSkaut;
using UserInterface.Commands;
using DatabaseManager;
using UserManager;
using System.Collections.ObjectModel;
using Supabase;
using System.Threading.Tasks;

namespace UserInterface.ViewModels.ModelRepresantations
{
	public class TransactionViewModel : EditableClass
	{
		private Transaction transaction;

        public long? Id { get => transaction.Id; set => transaction.Id = value; }
		public string Name { get => transaction.Name; set { transaction.Name = value; IsChanged = true; SaveRowCommand.RaiseCanExecuteChanged(); } }

        public TransactionTypeViewModel Type { get => new(transaction.Type, client); set { transaction.Type = value.getTransactionType(); IsChanged = true; SaveRowCommand.RaiseCanExecuteChanged(); } }
        public int Amount { get => transaction.Amount; set { transaction.Amount = value; IsChanged = true; SaveRowCommand.RaiseCanExecuteChanged(); } }
        public DateTime Date { get => transaction.Date; set { transaction.Date = value; IsChanged = true; SaveRowCommand.RaiseCanExecuteChanged(); } }

        public long EventId { get => transaction.EventId; set => transaction.EventId = value; }

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
                    dbTransaction.Amount, dbTransaction.Date.ToDateTime(TimeOnly.Parse("10:00 PM")), transactionTypes[dbTransaction.TypeId],
                    dbTransaction.EventId);

                result.Add(new TransactionViewModel(transaction, client));
            }

            return result;
        }

        public override async void SaveRow(object obj)
        {
            IsChanged = false;
            SaveRowCommand.RaiseCanExecuteChanged();

            if (Id == null)
                Id = await DBTransaction.CreateTransaction(Name, (long)Type.Id, Amount, DateOnly.FromDateTime(Date), EventId, client);
            else
                await DBTransaction.UpdateTransaction((long)Id, Name, (long)Type.Id, Amount, DateOnly.FromDateTime(Date), EventId, client);
        }

        public override async void DeleteRow(object obj)
        {
            if (Id != null)
                await DBTransaction.DeleteTransaction((long)Id, client);
        }

        public override bool CanSaveRow()
        {
            return Name != null && Name.Length > 0 && IsChanged && Amount >= 0 && Type != null;
        }

        public override bool CanDeleteRow()
        {
            return true;
        }
    }
}
