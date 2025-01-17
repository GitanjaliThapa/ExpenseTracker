namespace PersonalExpenseTracker.Model.Dto
{
    public class CreateDebtDto
    {
        public Guid Id { get; set; }

        public string DebtSource { get; set; }

        public decimal DebtAmount { get; set; }

        public DateTime? DueDate { get; set; }

        public bool IsCleard { get; set; }

        public bool IsActive { get; set; }

        public Guid TagId { get; set; }
    }
}