using DatabaseManager;
using MSkaut;
using System;
using System.Collections.ObjectModel;
using Supabase;
using UserManager;

namespace UserInterface.ViewModels.ModelRepresantations
{
	public class TransactionTypeViewModel : EditableClass
	{
        private TransactionType transactionType;
        public TransactionType TransactionType { get => transactionType; set => transactionType = value; }

        public long? Id { get => transactionType.Id; set => transactionType.Id = value; }
        public string Name { get => transactionType.Name; set { transactionType.Name = value; SaveRowCommand.RaiseCanExecuteChanged(); } }
        public string Description { get => transactionType.Description; set { transactionType.Description = value; SaveRowCommand.RaiseCanExecuteChanged(); } }

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
                TransactionType transactionType = new(dbTransactionType.Id, dbTransactionType.Name, dbTransactionType.Description);
                result.Add(new TransactionTypeViewModel(transactionType, client));
            }

            return result;
        }

        public TransactionType getTransactionType()
        {
            return transactionType;
        }

        public override void DeleteRow(object obj)
        {
            throw new NotImplementedException();
        }

        public override void SaveRow(object obj)
        {
            throw new NotImplementedException();
        }

        public override string ToString()
        {
            return transactionType.ToString();
        }

        public override bool CanSaveRow()
        {
            return Name != null && Name.Length > 0 && Description != null;
        }

        public override bool CanDeleteRow()
        {
            return false;
        }
    }
}
