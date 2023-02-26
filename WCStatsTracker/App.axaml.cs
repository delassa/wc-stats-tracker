using System;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using WCStatsTracker.Services;
using WCStatsTracker.ViewModels;
using WCStatsTracker.Views;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;

namespace WCStatsTracker;

public class App : Application
{
    public static ServiceProvider serviceProvider;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        /// Setup Logger
        using var log = new LoggerConfiguration().WriteTo.Console(theme: SystemConsoleTheme.Literate).WriteTo.Debug().MinimumLevel.Debug().CreateLogger();
        Log.Logger = log;
        Log.Information("Logger Configured");
        /// Hopefully this is a good place to insert DI

        CreateServiceProvider();

        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Line below is needed to remove Avalonia data validation.
            // Without this line you will get duplicate validations from both Avalonia and CT
            BindingPlugins.DataValidators.RemoveAt(0);
            desktop.MainWindow = new MainWindow
            {
                DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>()
            };
        }

        base.OnFrameworkInitializationCompleted();
    }


    /// <summary>
    ///     Creates a DI container for our services
    /// </summary>
    /// <returns>The service provider with services registered</returns>
    public static void CreateServiceProvider()
    {
        IServiceCollection services = new ServiceCollection();

        services.AddSingleton<WCDBContextFactory>();
        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<RunsPageViewModel>();
        services.AddSingleton<FlagsPageViewModel>();
        services.AddSingleton<StatsPageViewModel>();
        services.AddSingleton<OptionsPageViewModel>();

        serviceProvider = services.BuildServiceProvider();
    }
}