using Supabase;
using DatabaseManager;
using System.Collections.ObjectModel;

namespace MSkaut
{
    public class Transaction
    {
        public long? Id { get; set; }
        public string Name { get; set; }
        public TransactionType Type { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }

        public Transaction(string name, int amount, DateTime date, TransactionType type)
        {
            Id = null;
            this.Name = name;
            this.Amount = amount;
            this.Date = date;
            this.Type = type;
        }

        public Transaction(long id, string name, int amount, DateTime date, TransactionType type) : this(name, amount, date, type)
        {
            this.Id = id;
        }
    }
}
