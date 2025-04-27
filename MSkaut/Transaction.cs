using Supabase;
using DatabaseManager;

namespace MSkaut
{
    public class Transaction
    {
        private TransactionType Type { get; set; }
        public int Amount { get; private set; }
        public DateOnly Date { get; private set; }
        public bool IsExpense { get; private set; }

        public Transaction(int amount, DateOnly date, TransactionType type, bool isExpense)
        {
            this.Amount = amount;
            this.Date = date;
            this.Type = type;
            this.IsExpense = false;
        }

        public static async Task<List<Transaction>> GetEventTransactions(int eventId,
            Dictionary<int, TransactionType> transactionTypes, Client client)
        {
            List<DBTransaction> dbTransactions = await DBTransaction.GetEventTransactions(eventId, client);
            List<Transaction> result = new();

            foreach(DBTransaction dbTransaction in dbTransactions)
            {
                result.Add(new Transaction(dbTransaction.Amount, dbTransaction.Date,
                    transactionTypes[dbTransaction.TypeId], dbTransaction.IsExpense ));
            }

            return result;
        }
    }
}
