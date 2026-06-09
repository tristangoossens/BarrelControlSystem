using BarrelControlSystem.Backend.Models.Enums;

namespace BarrelControlSystem.Backend.Models;

public class RelayConfig
{
    public string Id => $"R{RelayPinNumber}_GPIO{GpioPinNumberBcm}";

    public int RelayPinNumber { get; set; }
    public int GpioPinNumberBcm { get; set; }
    public DeviceConfig ConnectedDevice { get; set; }
    
    // Timestamps
    public DateTime LastUsed { get; set; }
    public DateTime ConfigurationLastUpdated { get; set; }

    public override string ToString()
    {
        string deviceStr = ConnectedDevice?.ToString() ?? "No device connected";
        return $"Relay Pin: {RelayPinNumber} (GPIO: {GpioPinNumberBcm})\n" +
               $"  {deviceStr.Replace("\n", "\n  ")}\n" +
               $"  Last Used: {LastUsed}\n" +
               $"  Config Updated: {ConfigurationLastUpdated}";
    }
}
