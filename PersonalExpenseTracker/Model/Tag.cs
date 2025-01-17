namespace PersonalExpenseTracker.Model
{
    public class Tag
    {
        public Guid Id { get; set; }

        public string TagName { get; set; } 

        public bool IsActive { get; set; }
    }
}