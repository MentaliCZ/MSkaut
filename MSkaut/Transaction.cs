namespace MSkaut
{
    public class Transaction
    {
        private int id = -1;
        private TransactionType Type { get; set; }
        public uint Amount { get; private set; }
        public Date Date { get; private set; }
        private bool isExpense;

        public Transaction(uint amount, Date date, TransactionType type)
        {
            this.Amount = amount;
            this.Date = date;
            this.Type = type;
        }

        public void setIsExpense(bool isExpense)
        {
            this.isExpense = isExpense;
        }
    }
}
