using BarrelControlSystem.UI.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace BarrelControlSystem.UI.ViewModels.Configuration;

public partial class ConfigurationViewModel : ViewModelBase
{
    [ObservableProperty]
    private ViewModelBase _currentSubPage = new SystemConfigurationViewModel();

    [RelayCommand]
    private void GoToSystemConfiguration() => CurrentSubPage = new SystemConfigurationViewModel();

    [RelayCommand]
    private void GoToRelayConfiguration() => CurrentSubPage = new RelayConfigurationViewModel();
}