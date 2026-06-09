using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using BarrelControlSystem.Backend.Handlers;
using BarrelControlSystem.UI.Models;
using BarrelControlSystem.UI.ViewModels;

namespace BarrelControlSystem.UI.ViewModels.Configuration;

public partial class SystemConfigurationViewModel : ViewModelBase
{
    [ObservableProperty]
    private bool _simulateGpio;

    [ObservableProperty]
    private int _gpioPinCount;

    [ObservableProperty]
    private int _relayPinCount;

    [ObservableProperty]
    private string _logPath;

    public SystemConfigurationViewModel()
    {
        LoadSettings();
    }

    private void LoadSettings()
    {
        var settings = ConfigurationHandler.GetLatestSettings();
        SimulateGpio = settings.SimulateGpio;
        GpioPinCount = settings.GpioIoPinCount;
        RelayPinCount = settings.RelayPinCount;
    }

    [RelayCommand]
    private void Save()
    {
        var settings = ConfigurationHandler.GetLatestSettings();
        settings.SimulateGpio = SimulateGpio;
        settings.GpioIoPinCount = GpioPinCount;
        settings.RelayPinCount = RelayPinCount;
        ConfigurationHandler.Save();
        WeakReferenceMessenger.Default.Send(new ToastMessage("Settings saved successfully!"));
    }

    [RelayCommand]
    private void ResetToDefaults()
    {
        ConfigurationHandler.ResetToDefaults();
        LoadSettings();
        WeakReferenceMessenger.Default.Send(new ToastMessage("All settings reset to default configuration!"));
    }
}
