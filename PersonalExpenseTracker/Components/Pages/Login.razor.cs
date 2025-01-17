using Microsoft.AspNetCore.Components;
using PersonalExpenseTracker.Base;
using PersonalExpenseTracker.Model;

namespace PersonalExpenseTracker.Components.Pages
{
    public partial class Login :ComponentBase
    {
        private User Users { get; set; } = new();

        [Parameter] public Currency Currency { get; set; }

        private string ErrorMessage { get; set; } = string.Empty;


        private void HandleLogin()
        {
            if (UserService.Login(Users))
            {
                Nav.NavigateTo("/dashboard");
            }

            else
            {
                ErrorMessage = "userName or password is invalid";
            }
        }

    }
}