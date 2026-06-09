using BarrelControlSystem.Backend.Models;
using BarrelControlSystem.Backend.Models.Enums;
using CommunityToolkit.Mvvm.ComponentModel;
using System;

namespace BarrelControlSystem.UI.ViewModels.Configuration;

public partial class RelayConfigWrapper : ObservableObject
{
    private readonly RelayConfig _config;

    public RelayConfigWrapper(RelayConfig config)
    {
        _config = config;
    }

    public RelayConfig Model => _config;

    public string Id => _config.Id;
    public int RelayPinNumber => _config.RelayPinNumber;
    public int GpioPinNumberBcm => _config.GpioPinNumberBcm;
    
    public DeviceConfig ConnectedDevice => _config.ConnectedDevice;
    public DateTime LastUsed => _config.LastUsed;
    public DateTime ConfigurationLastUpdated => _config.ConfigurationLastUpdated;

    public bool CanToggle
    {
        get => _config.ConnectedDevice.Capabilities.HasFlag(DeviceCapabilityType.Toggle);
        set
        {
            if (value) _config.ConnectedDevice.Capabilities |= DeviceCapabilityType.Toggle;
            else _config.ConnectedDevice.Capabilities &= ~DeviceCapabilityType.Toggle;
            OnPropertyChanged();
        }
    }

    public bool CanPulse
    {
        get => _config.ConnectedDevice.Capabilities.HasFlag(DeviceCapabilityType.Pulse);
        set
        {
            if (value) _config.ConnectedDevice.Capabilities |= DeviceCapabilityType.Pulse;
            else _config.ConnectedDevice.Capabilities &= ~DeviceCapabilityType.Pulse;
            OnPropertyChanged();
        }
    }

    public bool CanHold
    {
        get => _config.ConnectedDevice.Capabilities.HasFlag(DeviceCapabilityType.Hold);
        set
        {
            if (value) _config.ConnectedDevice.Capabilities |= DeviceCapabilityType.Hold;
            else _config.ConnectedDevice.Capabilities &= ~DeviceCapabilityType.Hold;
            OnPropertyChanged();
        }
    }

    public bool CanStrobe
    {
        get => _config.ConnectedDevice.Capabilities.HasFlag(DeviceCapabilityType.Strobe);
        set
        {
            if (value) _config.ConnectedDevice.Capabilities |= DeviceCapabilityType.Strobe;
            else _config.ConnectedDevice.Capabilities &= ~DeviceCapabilityType.Strobe;
            OnPropertyChanged();
        }
    }
}
