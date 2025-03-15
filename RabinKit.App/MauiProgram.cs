using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui;
using RabinKit.App.Services;
using RabinKit.Database;
using RabinKit.Core;
using MudBlazor.Services;
using MudBlazor;
using System.Text;

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

            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            var isDevelopment = GetDebugEnvironment();

            var libval = Path.Combine(
                FileSystem.AppDataDirectory,
                "Database.db3");

            builder.Services.AddCore();
            builder.Services.AddLogging();
            builder.Services.AddSqLite(x => x.Path = libval,
                isDevelopment: isDevelopment);

            Migrate(builder.Services);

            builder.Services.AddMauiBlazorWebView();
            builder.Services.AddMudServices();
            builder.Services.AddMudMarkdownServices();
            builder.Services.AddScoped<ExceptionHandler>();
            builder.Services.AddScoped<BackupService>();
            builder.UseMauiCommunityToolkit();

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