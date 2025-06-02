using System.Collections.ObjectModel;
using DatabaseManager.Transaction;
using MSkaut;
using Supabase;

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

        public string DocumentName
        {
            get => transaction.DocumentName;
            set
            {
                transaction.DocumentName = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
            }
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
            List<TransactionEntity> dbTransactions = await TransactionFunc.GetEventTransactions(
                eventId,
                client
            );
            ObservableCollection<TransactionViewModel> result = new();

            foreach (TransactionEntity dbTransaction in dbTransactions)
            {
                Transaction transaction;
                if (dbTransaction.TypeId == null)
                {
                    transaction = new Transaction(
                        dbTransaction.Id,
                        dbTransaction.DocumentName,
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
                        dbTransaction.DocumentName,
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
            IsProcessing = true;
            SaveRowCommand.RaiseCanExecuteChanged();

            if (Id == null)
                Id = await TransactionFunc.CreateTransaction(
                    DocumentName,
                    Name,
                    (long)Type.Id,
                    Amount,
                    DateOnly.FromDateTime(Date),
                    EventId,
                    client
                );
            else
                await TransactionFunc.UpdateTransaction(
                    (long)Id,
                    DocumentName,
                    Name,
                    (long)Type.Id,
                    Amount,
                    DateOnly.FromDateTime(Date),
                    EventId,
                    client
                );

            IsProcessing = false;
        }

        public override async void DeleteRow(object obj)
        {
            IsProcessing = true;

            if (Id == null)
                return;

            await TransactionFunc.DeleteTransaction((long)Id, client);

            IsProcessing = false;
        }

        public override bool CanSaveRow()
        {
            return Name != null
                && Name.Length > 0
                && IsChanged
                && Amount >= 0
                && type.getTransactionType() != null
                && Name.Length <= 30
                && !IsProcessing;
        }

        public override bool CanDeleteRow()
        {
            return !IsProcessing;
        }
    }
}
