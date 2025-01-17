using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace PersonalExpenseTracker.Components.Layout
{
    public partial class Back
    {
        [Parameter] public string? NavigationUrl { get; set; }

        private async Task GoBack()
        {
            if (string.IsNullOrEmpty(NavigationUrl))
            {
                await JsRuntime.InvokeVoidAsync("history.back");
            }
            else
            {
                Nav.NavigateTo(NavigationUrl);
            }
        }
    }
}