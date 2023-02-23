using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Data.Core;
using Avalonia.Data.Core.Plugins;
using Avalonia.Markup.Xaml;
using Microsoft.Extensions.DependencyInjection;
using System;
using WCStatsTracker.Services;
using WCStatsTracker.ViewModels;
using WCStatsTracker.Views;

namespace WCStatsTracker
{
    public partial class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            /// Hopefully this is a good place to insert DI
            IServiceProvider serviceProvider = CreateServiceProvider();


            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                // Line below is needed to remove Avalonia data validation.
                // Without this line you will get duplicate validations from both Avalonia and CT
                BindingPlugins.DataValidators.RemoveAt(0);
                desktop.MainWindow = new MainWindow
                {
                    DataContext = serviceProvider.GetRequiredService<MainWindowViewModel>(),
                };
            }

            base.OnFrameworkInitializationCompleted();
        }


        /// <summary>
        /// Creates a DI container for our services
        /// </summary>
        /// <returns>The service provider with services registered</returns>
        public static IServiceProvider CreateServiceProvider()
        {
            IServiceCollection services = new ServiceCollection();

            services.AddSingleton<WCDBContextFactory>();
            services.AddSingleton<MainWindowViewModel>();
            services.AddSingleton<RunsPageViewModel>();
            services.AddSingleton<FlagsPageViewModel>();
            services.AddSingleton<StatsPageViewModel>();
            services.AddSingleton<OptionsPageViewModel>();

            return services.BuildServiceProvider();
        }

    }
}