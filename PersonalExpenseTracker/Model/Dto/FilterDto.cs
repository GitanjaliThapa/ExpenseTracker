namespace PersonalExpenseTracker.Model.Dto
{
    public class FilterDto
    {
        public string? Title { get; set; }  
        
        public DateTime? TransactionDate { get; set; }

        public DateTime? StartDate { get; set; }

        public DateTime? EndDate { get; set; }
    }
}