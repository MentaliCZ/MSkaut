using System.Collections.ObjectModel;
using DatabaseManager.TransactionType;
using MSkaut;
using Supabase;
using UserManager;

namespace UserInterface.ViewModels.ModelRepresantations
{
    public class TransactionTypeViewModel : EditableClass
    {
        private TransactionType transactionType;

        public long? Id
        {
            get => transactionType.Id;
            set => transactionType.Id = value;
        }
        public string Name
        {
            get => transactionType.Name;
            set
            {
                transactionType.Name = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
            }
        }
        public string Description
        {
            get => transactionType.Description;
            set
            {
                transactionType.Description = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
            }
        }
        public bool IsExpense
        {
            get => transactionType.IsExpense;
            set
            {
                transactionType.IsExpense = value;
                IsChanged = true;
                SaveRowCommand.RaiseCanExecuteChanged();
            }
        }
        public long? OwnerId
        {
            get => transactionType.OwnerId;
            set => transactionType.OwnerId = value;
        }

        public TransactionTypeViewModel(TransactionType transactionType, Client client)
            : base(client)
        {
            this.transactionType = transactionType;
        }

        public static async Task<
            ObservableCollection<TransactionTypeViewModel>
        > GetUsersTransactionTypes(User user, Client client)
        {
            List<TransactionTypeEntity> dbTransactionTypes =
                await TransactionTypeFunc.GetUsersTransactionTypes(user.Id, client);
            ObservableCollection<TransactionTypeViewModel> result = new();

            foreach (TransactionTypeEntity dbTransactionType in dbTransactionTypes)
            {
                TransactionType transactionType = new(
                    dbTransactionType.Id,
                    dbTransactionType.Name,
                    dbTransactionType.Description,
                    dbTransactionType.IsExpense,
                    dbTransactionType.OwnerId
                );
                result.Add(new TransactionTypeViewModel(transactionType, client));
            }

            return result;
        }

        public TransactionType getTransactionType()
        {
            return transactionType;
        }

        public override async void DeleteRow(object obj)
        {
            if (Id == null)
                return;

            IsProcessing = true;

            await TransactionTypeFunc.DeleteTransactionType((long)Id, client);

            IsProcessing = false;
        }

        public override async void SaveRow(object obj)
        {
            IsChanged = false;
            IsProcessing = true;
            SaveRowCommand.RaiseCanExecuteChanged();

            if (Id == null)
                Id = await TransactionTypeFunc.CreateTransactionType(
                    Name,
                    Description,
                    IsExpense,
                    (long)OwnerId,
                    client
                );
            else
                await TransactionTypeFunc.UpdateTransactionType(
                    (long)Id,
                    Name,
                    Description,
                    IsExpense,
                    OwnerId,
                    client
                );

            IsProcessing = false;
        }

        public override string ToString()
        {
            return transactionType.ToString();
        }

        public override bool CanSaveRow()
        {
            return Name != null
                && Name.Length > 0
                && Description != null
                && IsChanged
                && OwnerId != null
                && Name.Length <= 30
                && !IsProcessing;
        }

        public override bool CanDeleteRow()
        {
            return OwnerId != null && !IsProcessing;
        }
    }
}
