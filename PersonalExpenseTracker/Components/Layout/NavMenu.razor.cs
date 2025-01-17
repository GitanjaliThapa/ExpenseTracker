using Microsoft.AspNetCore.Components;

namespace PersonalExpenseTracker.Components.Layout
{
    public partial class NavMenu : ComponentBase
    {
        private bool IslogOut { get; set; } = false;

        private async void ShowLogoutConfirmation()
        {
            IslogOut = true;
        }

        private async void HideLogoutConfirmation()
        {
            IslogOut = false;
        }

        #region Logout
        private async void Logout()
        {
            Nav.NavigateTo("/login");
        }
        #endregion
    }
}