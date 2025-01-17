namespace PersonalExpenseTracker.Model.Dto
{
    public class GetTagDto
    {
        public Guid Id { get; set; } = new Guid();

        public string TagName { get; set; }

        public bool IsActive { get; set; }
    }
}