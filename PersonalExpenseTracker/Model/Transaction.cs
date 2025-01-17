using PersonalExpenseTracker.Base;

namespace PersonalExpenseTracker.Model
{
    public class Transaction
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public decimal TransactionAmount {  get; set; }

        public DateTime? TransactionDate { get; set; }

        public int TransactionType { get; set; }

        public bool IsActive { get; set; }

        public string Remarks { get; set; }

        public Guid TagId { get; set; }

        public Tag Tag { get; set; }
    }
}