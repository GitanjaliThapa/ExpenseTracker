using ApexCharts;
using Microsoft.AspNetCore.Components;

namespace PersonalExpenseTracker.Components.Layout
{
    public partial class PieChart<T> : ComponentBase where T : class
    {
        [Parameter]
        public List<T> Items { get; set; } = new();

        [Parameter]
        public Func<T, string> XValue { get; set; }

        [Parameter]
        public Func<T, decimal?> YValue { get; set; }

        private ApexChartOptions<T> PieChartOptions { get; set; } = new();

        protected override void OnInitialized()
        {
            PieChartOptions = new ApexChartOptions<T>
            {
                PlotOptions = new PlotOptions
                {
                    Pie = new PlotOptionsPie
                    {
                        ExpandOnClick = true // Optionally allow slices to expand on click
                    }
                },
                Labels = Items.Select(XValue).ToList() // Sets labels based on XValue
            };
        }
    }
}