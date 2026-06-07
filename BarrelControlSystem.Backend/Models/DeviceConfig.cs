using BarrelControlSystem.Backend.Models.Enums;

namespace BarrelControlSystem.Backend.Models;

public class DeviceConfig
{
    public int Id { get; init; }
    public string Name { get; set; } = string.Empty;
    public DeviceType Type { get; set; }
    public string Description { get; set; } = string.Empty;
    public DeviceCapabilityType Capabilities { get; set; }

    public override string ToString()
    {
        return $"Device: {Name} (ID: {Id})\n" +
               $"  Type: {Type}\n" +
               $"  Capabilities: {Capabilities}\n" +
               $"  Description: {Description}";
    }
}