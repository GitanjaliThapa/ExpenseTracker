using PersonalExpenseTracker.Abstraction;
using PersonalExpenseTracker.Model;
using PersonalExpenseTracker.Services.Interface;

namespace PersonalExpenseTracker.Services
{
    public class UserService : UserBase<User> ,IUser
    {
        private List<User> _users;

        public const string SeedUsername = "admin";
        public const string SeedPassword = "password";

        public UserService() : base("User.json")
        {
            _users = LoadItems();

            if (!_users.Any())
            {
                _users.Add(new User { UserName = SeedUsername, Password = SeedPassword });
                SaveItems(_users);
            }

        }

        public bool Login(User user)
        {
            if (string.IsNullOrEmpty(user.UserName) || string.IsNullOrEmpty(user.Password))
            {
                return false;
            }

            return  _users.Any(u => u.UserName == user.UserName && u.Password == user.Password);
        }
    }
}