using System;
using System.Collections.ObjectModel;
using BarrelControlSystem.Backend.Handlers;
using BarrelControlSystem.Backend.Models;
using BarrelControlSystem.Backend.Models.Enums;
using BarrelControlSystem.UI.ViewModels;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using BarrelControlSystem.UI.Models;

namespace BarrelControlSystem.UI.ViewModels.Configuration;

public partial class RelayConfigurationViewModel : ViewModelBase
{
    public ObservableCollection<RelayConfigWrapper> RelayConfigs { get; }
    
    public Array DeviceTypes => Enum.GetValues(typeof(DeviceType));

    public RelayConfigurationViewModel()
    {
        var settings = ConfigurationHandler.GetLatestSettings();
        RelayConfigs = new ObservableCollection<RelayConfigWrapper>();
        foreach (var relay in settings.RelayBoardConfig)
        {
            RelayConfigs.Add(new RelayConfigWrapper(relay));
        }
    }

    [RelayCommand]
    private void SaveRelay(RelayConfigWrapper relay)
    {
        ConfigurationHandler.Save();
        
        // Refresh the RelayConfigs list to reflect any changes
        RefreshRelayConfigs();

        WeakReferenceMessenger.Default.Send(new ToastMessage($"Relay {relay.Id} settings saved!"));
    }

    [RelayCommand]
    private void ResetRelay(RelayConfigWrapper relay)
    {
        ConfigurationHandler.ResetRelayConfig(relay.Model.RelayPinNumber);
        
        // Refresh the RelayConfigs list
        RefreshRelayConfigs();
        WeakReferenceMessenger.Default.Send(new ToastMessage($"Relay {relay.Id} settings reset!"));
    }
    
    private void RefreshRelayConfigs()
    {
        var settings = ConfigurationHandler.GetLatestSettings();
        RelayConfigs.Clear();
        foreach (var r in settings.RelayBoardConfig)
        {
            RelayConfigs.Add(new RelayConfigWrapper(r));
        }
    }
}
