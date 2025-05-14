using System;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using DatabaseManager;
using MSkaut;
using Supabase;
using UserInterface.Commands;
using UserManager;

namespace UserInterface.ViewModels.ModelRepresantations
{
    public class TransactionViewModel : EditableClass
    {
        private Transaction transaction;

        public long? Id
        {
            get => transaction.Id;
            set => transaction.Id = value;
        }
        public string Name
        {
            get => transaction.Name;
            set
            {
                transaction.Name = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
            }
        }

        private TransactionTypeViewModel type;
        public TransactionTypeViewModel Type
        {
            get => type;
            set
            {
                type = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
            }
        }

        public int Amount
        {
            get => transaction.Amount;
            set
            {
                transaction.Amount = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
            }
        }
        public DateTime Date
        {
            get => transaction.Date;
            set
            {
                transaction.Date = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
            }
        }

        public long EventId
        {
            get => transaction.EventId;
            set => transaction.EventId = value;
        }

        public TransactionViewModel(Transaction transaction, Client client)
            : base(client)
        {
            this.transaction = transaction;

            this.Type = new(transaction.Type, client);
            IsChanged = false;
        }

        public TransactionViewModel(
            Transaction transaction,
            Client client,
            Dictionary<long, TransactionTypeViewModel> transactionTypes
        )
            : base(client)
        {
            this.transaction = transaction;

            this.Type = transactionTypes[(long)transaction.Type.Id];
            IsChanged = false;
        }

        public static async Task<ObservableCollection<TransactionViewModel>> GetEventTransactions(
            long eventId,
            Dictionary<long, TransactionTypeViewModel> transactionTypes,
            Client client
        )
        {
            List<DBTransaction> dbTransactions = await DBTransaction.GetEventTransactions(
                eventId,
                client
            );
            ObservableCollection<TransactionViewModel> result = new();

            foreach (DBTransaction dbTransaction in dbTransactions)
            {
                Transaction transaction;
                if (dbTransaction.TypeId == null)
                {
                    transaction = new Transaction(
                        dbTransaction.Id,
                        dbTransaction.Name,
                        dbTransaction.Amount,
                        dbTransaction.Date.ToDateTime(TimeOnly.Parse("10:00 PM")),
                        null,
                        dbTransaction.EventId
                    );

                    result.Add(new TransactionViewModel(transaction, client));
                }
                else
                {
                    transaction = new Transaction(
                        dbTransaction.Id,
                        dbTransaction.Name,
                        dbTransaction.Amount,
                        dbTransaction.Date.ToDateTime(TimeOnly.Parse("10:00 PM")),
                        transactionTypes[(long)dbTransaction.TypeId].getTransactionType(),
                        dbTransaction.EventId
                    );

                    result.Add(new TransactionViewModel(transaction, client, transactionTypes));
                }
            }

            return result;
        }

        public override async void SaveRow(object obj)
        {
            IsChanged = false;
            SaveRowCommand.RaiseCanExecuteChanged();

            if (Id == null)
                Id = await DBTransaction.CreateTransaction(
                    Name,
                    (long)Type.Id,
                    Amount,
                    DateOnly.FromDateTime(Date),
                    EventId,
                    client
                );
            else
                await DBTransaction.UpdateTransaction(
                    (long)Id,
                    Name,
                    (long)Type.Id,
                    Amount,
                    DateOnly.FromDateTime(Date),
                    EventId,
                    client
                );
        }

        public override async void DeleteRow(object obj)
        {
            if (Id == null)
                return;

            await DBTransaction.DeleteTransaction((long)Id, client);
        }

        public override bool CanSaveRow()
        {
            return Name != null
                && Name.Length > 0
                && IsChanged
                && Amount >= 0
                && type.getTransactionType() != null
                && Name.Length <= 30;
        }

        public override bool CanDeleteRow()
        {
            return true;
        }
    }
}
