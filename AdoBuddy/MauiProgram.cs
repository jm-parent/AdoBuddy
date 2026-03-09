using AdoBuddy.Services;
using AdoBuddy.ViewModels;
using AdoBuddy.Views;
using Microsoft.Extensions.Logging;

namespace AdoBuddy
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            builder.Services.AddHttpClient();

            builder.Services.AddSingleton<IAzureDevOpsService, AzureDevOpsService>();

            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<MainPage>();

            builder.Services.AddTransient<PipelinesViewModel>();
            builder.Services.AddTransient<PipelinesPage>();

            builder.Services.AddTransient<PullRequestsViewModel>();
            builder.Services.AddTransient<PullRequestsPage>();

            return builder.Build();
        }
    }
}
