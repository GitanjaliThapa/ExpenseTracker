using Microsoft.AspNetCore.Components;
using MudBlazor;
using Color = MudBlazor.Color;


namespace PersonalExpenseTracker.Components.Layout
{
    public partial class ActivationStatus
    {
        [Parameter] public bool? IsActive { get; set; }

        [Parameter] public Variant Variant { get; set; } = Variant.Outlined;

        private Color GetButtonColor()
        {
            return IsActive switch
            {
                true => Color.Success,
                false => Color.Error,
                _ => Color.Warning
            };
        }

        private string GetActivationStatus()
        {
            return IsActive switch
            {
                true => "Active",
                false => "Inactive",
                _ => "Pending"
            };
        }

    }
}