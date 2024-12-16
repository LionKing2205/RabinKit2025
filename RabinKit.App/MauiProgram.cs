using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using RabinKit.App.Services;
using RabinKit.Database;
using RabinKit.Core;
using MudBlazor.Services;
using MudBlazor;

namespace RabinKit.App
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder.UseMauiApp<App>().ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            });
            

            var isDevelopment = GetDebugEnvironment();

            var libval = Path.Combine(
                FileSystem.AppDataDirectory,
                "Database.db3");

            builder.Services.AddCore();
            builder.Services.AddLogging();
            builder.Services.AddSqLite(x => x.Path = Path.Combine(
                FileSystem.AppDataDirectory,
                "Database.db3"),
                isDevelopment: isDevelopment);

            Migrate(builder.Services);

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();
            builder.Services.AddMudMarkdownServices();
            builder.UseMauiCommunityToolkit();
            builder.Services.AddScoped<ExceptionHandler>();
            builder.Services.AddScoped<BackupService>();


            if (isDevelopment)
            {
                builder.Services.AddBlazorWebViewDeveloperTools();
                builder.Logging.AddDebug();
            }

            return builder.Build();
        }

        private static bool GetDebugEnvironment()
            =>
#if DEBUG
                true;
#else
                false;
#endif
        private static void Migrate(IServiceCollection serviceCollection)
        {
            using var services = serviceCollection.BuildServiceProvider();

            var migrator = services.GetRequiredService<DbMigrator>();
            migrator.MigrateAsync().GetAwaiter().GetResult();
        }
    }
}