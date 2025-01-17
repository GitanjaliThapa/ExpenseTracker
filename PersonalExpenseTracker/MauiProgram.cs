using Blazored.Toast;
using Microsoft.Extensions.Logging;
using MudBlazor.Services;
using PersonalExpenseTracker.Services;
using PersonalExpenseTracker.Services.Interface;

namespace PersonalExpenseTracker
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
            // service inject 
            builder.Services.AddScoped<IUser, UserService>();
            builder.Services.AddScoped<ITag, TagService>();
            builder.Services.AddScoped<IDebt, DebtService>();
            builder.Services.AddScoped<ITransaction, TransactionService>();
            // builder.Services.AddScoped<ISnackbarService>();
            builder.Services.AddBlazorWebViewDeveloperTools();
            builder.Services.AddMudServices();
            builder.Services.AddBlazoredToast();
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}