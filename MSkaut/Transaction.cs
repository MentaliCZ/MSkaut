namespace MSkaut
{
    public class Transaction
    {
        private TransactionType Type { get; set; }
        public uint Amount { get; private set; }
        public Date Date { get; private set; }
        public bool IsExpense { get; private set; }

        public Transaction(uint amount, Date date, TransactionType type)
        {
            this.Amount = amount;
            this.Date = date;
            this.Type = type;
            this.IsExpense = false;
        }
    }
}
