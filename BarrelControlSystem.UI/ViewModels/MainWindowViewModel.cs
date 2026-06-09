using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using BarrelControlSystem.UI.Models;
using BarrelControlSystem.UI.ViewModels.Configuration;
using BarrelControlSystem.UI.ViewModels.Dashboard;
using BarrelControlSystem.UI.ViewModels.Home;
using BarrelControlSystem.UI.ViewModels.Programs;

namespace BarrelControlSystem.UI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public MainWindowViewModel()
    {
        WeakReferenceMessenger.Default.Register<ToastMessage>(this, (r, m) => ShowToast(m.Message));
    }

    [ObservableProperty]
    private ViewModelBase _currentPage = new HomeViewModel();

    [ObservableProperty]
    private bool _isToastVisible;

    [ObservableProperty]
    private string? _toastMessage;

    public void ShowToast(string message)
    {
        ToastMessage = message;
        IsToastVisible = true;
        
        // Hide after 3 seconds
        System.Threading.Tasks.Task.Delay(3000).ContinueWith(_ => IsToastVisible = false, System.Threading.Tasks.TaskScheduler.FromCurrentSynchronizationContext());
    }

    [RelayCommand]
    private void GoToHome() => CurrentPage = new HomeViewModel();

    [RelayCommand]
    private void GoToDashboard() => CurrentPage = new DashboardViewModel();

    [RelayCommand]
    private void GoToPrograms() => CurrentPage = new ProgramsViewModel();

    [RelayCommand]
    private void GoToConfiguration() => CurrentPage = new ConfigurationViewModel();
}
