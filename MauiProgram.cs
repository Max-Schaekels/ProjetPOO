using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;
using ProjetPOO.Utilities.DataAccess;
using ProjetPOO.Utilities.DataAccess.Files;
using ProjetPOO.Utilities.Interfaces;
using ProjetPOO.Utilities.Services;
using ProjetPOO.View;
using ProjetPOO.ViewModel;

namespace ProjetPOO
{
    public static class MauiProgram
    {
        private const string CONFIG_HOME_CSV = @"C:\ProjetPOO\Max-Schaekels\ProjetPOO\Configuration\Datas\Config.local.txt";
        private const string CONFIG_PORT_CSV = @"C:\POO\ProjetPOO\Configuration\Datas\Config.local.txt";

        private const string CONFIG_HOME_JSON = @"C:\ProjetPOO\Max-Schaekels\ProjetPOO\Configuration\Datas\ConfigJson.local.txt";
        private const string CONFIG_PORT_JSON = @"C:\POO\ProjetPOO\Configuration\Datas\ConfigJson.local.txt";
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });

            builder.Services.AddSingleton<DataFilesManager>(new DataFilesManager(CONFIG_PORT_JSON));

            //Singleton for AlertServiceDisplay
            builder.Services.AddSingleton<IAlertService, AlertServiceDisplay>();
            builder.Services.AddSingleton<IDataAccess, DataAccessJsonFile>();

            builder.Services.AddTransient<MainPageViewModel>();
            builder.Services.AddTransient<MainPage>();

            builder.Services.AddTransient<ScenarioListViewModel>();
            builder.Services.AddTransient<ScenarioListPage>();

            builder.Services.AddTransient<ScenarioEditorViewModel>();
            builder.Services.AddTransient<ScenarioEditorPage>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
