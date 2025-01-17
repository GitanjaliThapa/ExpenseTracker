namespace PersonalExpenseTracker.Model.Dto
{
    public class CreateTransactionDto
    {
        public Guid Id { get; set; }

        public string? Title { get; set; }

        public decimal TransactionAmount { get; set; }

        public DateTime? TransactionDate { get; set; }

        public int TransactionType { get; set; }

        public string Remarks { get; set; }

        public Guid TagId { get; set; }

        public Tag Tag { get; set; }
    }
}