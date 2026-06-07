namespace BarrelControlSystem.Backend.Models.Enums;

[Flags]
public enum DeviceCapabilityType
{
    None,
    Toggle,
    Pulse,          
    Hold,
    Strobe,
}