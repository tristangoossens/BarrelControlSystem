using Newtonsoft.Json;
using System.Linq;
using BarrelControlSystem.Backend.Models;
using BarrelControlSystem.Backend.Models.Enums;

namespace BarrelControlSystem.Backend.Handlers;

public static class ConfigurationHandler
{
    private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BarrelControlSystemSettings.json");
    private static readonly JsonSerializerSettings JsonSettings = new() { Formatting = Formatting.Indented };
    
    private static BarrelControlSystemSettings _settings = new();

    public static BarrelControlSystemSettings GetLatestSettings()
    {
        Load();
        LoggingHandler.LogInfo("Successfully loaded latest configuration.");
        return _settings;
    }

    public static void Load()
    {
        try
        {
            if (File.Exists(ConfigFilePath))
            {
                string json = File.ReadAllText(ConfigFilePath);
                _settings = JsonConvert.DeserializeObject<BarrelControlSystemSettings>(json) ?? CreateDefault();
            }
            else
            {
                LoggingHandler.LogWarning("Configuration file not found. Creating default configuration.");
                _settings = CreateDefault();
                Save();
            }
        }
        catch (Exception ex)
        {
            LoggingHandler.LogError($"Failed to load configuration: {ex.Message}");
            throw;
        }
    }

    public static void Save()
    {
        _settings.LastUpdated = DateTime.Now;
        string json = JsonConvert.SerializeObject(_settings, JsonSettings);
        File.WriteAllText(ConfigFilePath, json);
       LoggingHandler.LogInfo("System settings saved successfully.");
    }

    public static void ResetToDefaults()
    {
        _settings = CreateDefault();
        Save();
        LoggingHandler.LogInfo("Configuration has been reset to defaults.");
    }

    public static void ResetRelayConfig(int pinNumber)
    {
        var relayConfig = _settings.RelayBoardConfig.FirstOrDefault(r => r.RelayPinNumber == pinNumber);
        if (relayConfig != null)
        {
            int index = _settings.RelayBoardConfig.IndexOf(relayConfig);
            _settings.RelayBoardConfig[index] = GetDefaultRelayConfig(pinNumber);
            Save();
            LoggingHandler.LogInfo($"Relay configuration for pin {pinNumber} has been reset to defaults.");
        }
        else
        {
            LoggingHandler.LogWarning($"Attempted to reset non-existent relay configuration for pin {pinNumber}.");
        }
    }

    private static BarrelControlSystemSettings CreateDefault()
    {
        var settings = new BarrelControlSystemSettings
        {
            SimulateGpio = true,
            GpioIoPinCount = 26,
            RelayPinCount = 16,
            LastUpdated = DateTime.Now,
            RelayBoardConfig = []
        };

        // Create defaults for the amount of available relay pins
        for (int i = 1; i <= settings.RelayPinCount; i++)
        {
            settings.RelayBoardConfig.Add(GetDefaultRelayConfig(i));
        }

        return settings;
    }

    private static RelayConfig GetDefaultRelayConfig(int pinNumber)
    {
        return new RelayConfig
        {
            RelayPinNumber = pinNumber,
            GpioPinNumberBcm = pinNumber,
            ConnectedDevice = new DeviceConfig
            {
                Id = pinNumber,
                Name = $"DefaultDevice{pinNumber}",
                Type = DeviceType.Unknown,
                Description = $"Default description for device on relay pin {pinNumber}",
            }
        };
    }
}