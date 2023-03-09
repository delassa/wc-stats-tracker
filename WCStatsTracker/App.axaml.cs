using System;
using System.IO;
using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using WCStatsTracker.Services.DataAccess;
using WCStatsTracker.ViewModels;
using WCStatsTracker.Views;
using LiveChartsCore;
using LiveChartsCore.SkiaSharpView;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;
using WCStatsTracker.Models;

namespace WCStatsTracker;

public class App : Application
{
    public static ServiceProvider serviceProvider;
    public static IConfigurationRoot _configuration;

    public override void Initialize()
    {
        AvaloniaXamlLoader.Load(this);
    }

    public override void OnFrameworkInitializationCompleted()
    {
        // Setup Logger
        _configuration = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build();

        var log = new LoggerConfiguration()
            .ReadFrom.Configuration(_configuration)
            .CreateLogger();

        Log.Logger = log;
        Log.Information("Logger Configured");

        // Configure Livecharts
        LiveCharts.Configure(config => config
            .AddSkiaSharp()
            .AddDefaultMappers()
            .AddDarkTheme()
            .HasMap<WcRun>((run, point) =>
            {
                point.PrimaryValue = run.RunLength.Ticks;
                point.SecondaryValue = point.Context.Entity.EntityIndex;
            })
        );
        if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
        {
            // Put this here to not interfere with design view
            CreateServiceProvider();
            var context = serviceProvider.GetRequiredService<WcDbContext>();
            context.GetInfrastructure().GetService<IMigrator>().Migrate();
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
        services.AddSingleton<TimingStatsViewModel>();
        services.AddSingleton<CharacterStatsViewModel>();
        services.AddTransient<IUnitOfWork, UnitOfWork>();

        // Get a configuration

        // Database resides in current working dir

        var fixedConnectionString = _configuration.GetConnectionString("Default")
            .Replace("{AppDir}", AppDomain.CurrentDomain.BaseDirectory);

        // Add in the Db Context
        // enable logging on our db and sensitive data logging for development
        services.AddDbContext<WcDbContext>(options => options
            .LogTo(Log.Logger.Information, LogLevel.Information).EnableSensitiveDataLogging()
            .UseLazyLoadingProxies()
            .UseSqlite(fixedConnectionString));

        serviceProvider = services.BuildServiceProvider();
    }
}
