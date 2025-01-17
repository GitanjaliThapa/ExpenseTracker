using PersonalExpenseTracker.Model;

namespace PersonalExpenseTracker.Services.Interface
{
    public interface IUser
    {
        bool Login(User user);
    }
}