using PersonalExpenseTracker.Base;

namespace PersonalExpenseTracker.Model
{
    public class User
    {
        public Guid UserId { get; set; } = new Guid();

        public string UserName { get; set; }    

        public string Password { get; set; }

        public Currency Currency { get; set; }
    }
}