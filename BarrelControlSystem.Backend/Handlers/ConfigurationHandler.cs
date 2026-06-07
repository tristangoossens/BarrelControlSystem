using Newtonsoft.Json;
using BarrelControlSystem.Backend.Models;
using BarrelControlSystem.Backend.Models.Enums;

namespace BarrelControlSystem.Backend.Handlers;

public static class ConfigurationHandler
{
    private static readonly string ConfigFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "BarrelControlSystemSettings.json");
    private static readonly JsonSerializerSettings JsonSettings = new() { Formatting = Formatting.Indented };
    
    public static BarrelControlSystemSettings Settings { get; private set; } = new();

    static ConfigurationHandler()
    {
        Load();
    }

    public static void Load()
    {
        try
        {
            if (File.Exists(ConfigFilePath))
            {
                string json = File.ReadAllText(ConfigFilePath);
                Settings = JsonConvert.DeserializeObject<BarrelControlSystemSettings>(json) ?? CreateDefault();
                LoggingHandler.LogInfo("Configuration loaded successfully.");
            }
            else
            {
                LoggingHandler.LogWarning("Configuration file not found. Creating default configuration.");
                Settings = CreateDefault();
                Save();
            }
        }
        catch (Exception ex)
        {
            LoggingHandler.LogError($"Failed to load configuration: {ex.Message}");
            Settings = CreateDefault();
        }
    }

    public static void Save()
    {
        try
        {
            string json = JsonConvert.SerializeObject(Settings, JsonSettings);
            File.WriteAllText(ConfigFilePath, json);
            LoggingHandler.LogInfo("System settings saved successfully.");
        }
        catch (Exception ex)
        {
            LoggingHandler.LogError($"Failed to save system settings: {ex.Message}");
        }
    }

    private static BarrelControlSystemSettings CreateDefault()
    {
        var settings = new BarrelControlSystemSettings
        {
            SimulateGpio = true,
            GpioIoPinCount = 26,
            RelayPinCount = 16,
            RelayBoardConfig = []
        };

        // Create defaults for the amount of available relay pins
        for (int i = 1; i <= settings.RelayPinCount; i++)
        {
            settings.RelayBoardConfig.Add(new RelayConfig
            {
                PinNumber = i,
                ConnectedDevice = new DeviceConfig
                {
                    Id = i,
                    Name = $"DefaultDevice{i}",
                    Type = DeviceType.Unknown,
                    Description = $"Default description for device on relay pin {i}",
                    Capabilities = DeviceCapabilityType.None
                },
                ConfigurationLastUpdated = DateTime.Now,
                LastUsed = DateTime.Now
            });
        }

        return settings;
    }
}