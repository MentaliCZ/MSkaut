using DatabaseManager;
using MSkaut;
using System;
using System.Collections.ObjectModel;
using Supabase;
using UserManager;
using Microsoft.Extensions.Logging;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading.Tasks;
using System.Windows;

namespace UserInterface.ViewModels.ModelRepresantations
{
	public class TransactionTypeViewModel : EditableClass
	{
        private TransactionType transactionType;

        public long? Id { get => transactionType.Id; set => transactionType.Id = value; }
        public string Name { get => transactionType.Name; set { transactionType.Name = value; IsChanged = true; SaveRowCommand.RaiseCanExecuteChanged(); } }
        public string Description { get => transactionType.Description; set { transactionType.Description = value; IsChanged = true; SaveRowCommand.RaiseCanExecuteChanged(); } }
        public bool IsExpense { get => transactionType.IsExpense; set { transactionType.IsExpense = value; IsChanged = true; SaveRowCommand.RaiseCanExecuteChanged(); } }
        public long? OwnerId { get => transactionType.OwnerId; set => transactionType.OwnerId = value; }

        public TransactionTypeViewModel(TransactionType transactionType, Client client) : base(client)
		{
            this.transactionType = transactionType;
		}

        public static async Task<ObservableCollection<TransactionTypeViewModel>> GetUsersTransactionTypes(User user, Client client)
        {
            List<DBTransactionType> dbTransactionTypes = await DBTransactionType.GetUsersTransactionTypes(user.Id, client);
            ObservableCollection<TransactionTypeViewModel> result = new();

            foreach (DBTransactionType dbTransactionType in dbTransactionTypes)
            {
                TransactionType transactionType = new(dbTransactionType.Id, dbTransactionType.Name, dbTransactionType.Description, dbTransactionType.IsExpense, dbTransactionType.OwnerId);
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

            var result = MessageBox.Show(
                "Are you sure you want to delete this type?",
                "Confirm Delete",
                MessageBoxButton.YesNo,
                MessageBoxImage.Warning);

            if (result == MessageBoxResult.Yes)
                await DBTransactionType.DeleteTransactionType((long)Id, client);
        }

        public override async void SaveRow(object obj)
        {
            IsChanged = false;
            SaveRowCommand.RaiseCanExecuteChanged();

            if (Id == null)
                Id = await DBTransactionType.CreateTransactionType(Name, Description, IsExpense, (long)OwnerId, client);
            else
                await DBTransactionType.UpdateTransactionType((long)Id, Name, Description, IsExpense, OwnerId, client);
        }

        public override string ToString()
        {
            return transactionType.ToString();
        }

        public override bool CanSaveRow()
        {
            return Name != null && Name.Length > 0 && Description != null && IsChanged && OwnerId != null && Name.Length <= 30;
        }

        public override bool CanDeleteRow()
        {
            return OwnerId != null;
        }

    }
}
