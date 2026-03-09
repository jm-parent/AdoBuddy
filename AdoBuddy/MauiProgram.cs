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

            // Services
            builder.Services.AddSingleton<ICredentialStore, CredentialStore>();
            builder.Services.AddSingleton<IAzureDevOpsService, AzureDevOpsService>();
            builder.Services.AddSingleton<ProjectContext>();

            // Shell & App
            builder.Services.AddSingleton<AppShell>();

            // ViewModels
            builder.Services.AddTransient<MainViewModel>();
            builder.Services.AddTransient<LoginViewModel>();
            builder.Services.AddTransient<ProjectsViewModel>();
            builder.Services.AddTransient<PipelinesViewModel>();
            builder.Services.AddTransient<PullRequestsViewModel>();
            builder.Services.AddTransient<WorkItemsViewModel>();

            // Pages
            builder.Services.AddTransient<MainPage>();
            builder.Services.AddTransient<LoginPage>();
            builder.Services.AddTransient<ProjectsPage>();
            builder.Services.AddTransient<PipelinesPage>();
            builder.Services.AddTransient<PullRequestsPage>();
            builder.Services.AddTransient<WorkItemsPage>();

            return builder.Build();
        }
    }
}
