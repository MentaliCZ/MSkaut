using System.Collections.ObjectModel;
using DatabaseManager;
using Supabase;

namespace MSkaut
{
    public class Transaction
    {
        public long? Id { get; set; }
        public string DocumentName { get; set; }
        public string Name { get; set; }
        public TransactionType Type { get; set; }
        public int Amount { get; set; }
        public DateTime Date { get; set; }
        public long EventId { get; set; }

        public Transaction(
            string documentName,
            string name,
            int amount,
            DateTime date,
            TransactionType type,
            long eventId
        )
        {
            Id = null;
            this.DocumentName = documentName;
            this.Name = name;
            this.Amount = amount;
            this.Date = date;
            this.Type = type;
            this.EventId = eventId;
        }

        public Transaction(
            long id,
            string documentName,
            string name,
            int amount,
            DateTime date,
            TransactionType type,
            long eventId
        )
            : this(documentName, name, amount, date, type, eventId)
        {
            this.Id = id;
        }
    }
}
