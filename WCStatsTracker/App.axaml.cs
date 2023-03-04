using System;
using System.Configuration;
using System.Diagnostics;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Serilog;
using Serilog.Sinks.SystemConsole.Themes;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.ViewModels;
using WCStatsTracker.Views;
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
        // Setup Logger
        using var log = new LoggerConfiguration().WriteTo.Console(theme: SystemConsoleTheme.Literate).WriteTo.Debug()
            .MinimumLevel.Debug().CreateLogger();
        Log.Logger = log;
        Log.Information("Logger Configured");


        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Put this here to not interfere with design view
            CreateServiceProvider();
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

        services.AddSingleton<MainWindowViewModel>();
        services.AddSingleton<RunsListViewModel>();
        services.AddSingleton<RunsAddViewModel>();
        services.AddSingleton<FlagsPageViewModel>();
        services.AddSingleton<StatsPageViewModel>();
        services.AddSingleton<OptionsPageViewModel>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        // Database resides in current working dir
        var fixedConnectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString
            .Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

        // Add in the Db Context
        // enable logging on our db and sensitive data logging for development
        services.AddDbContext<WcDbContext>(options => options
            .LogTo(message => Debug.WriteLine(message)).EnableSensitiveDataLogging()
            .UseSqlite(fixedConnectionString));


        serviceProvider = services.BuildServiceProvider();
    }
}