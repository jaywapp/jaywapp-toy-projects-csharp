using System.Windows;
using AIInstaller.App.Services;
using AIInstaller.App.ViewModels;

namespace AIInstaller.App;

public partial class App : Application
{
    protected override async void OnStartup(StartupEventArgs e)
    {
        base.OnStartup(e);

        InstallerDashboardService dashboardService = new();
        MainViewModel viewModel = new(dashboardService);

        MainWindow mainWindow = new()
        {
            DataContext = viewModel
        };

        mainWindow.Show();
        await viewModel.InitializeAsync(CancellationToken.None);
    }
}
