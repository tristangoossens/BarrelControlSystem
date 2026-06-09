namespace BarrelControlSystem.Backend.Models.Enums;

[Flags]
public enum DeviceCapabilityType
{
    None = 0,
    Toggle = 1,
    Pulse = 2,          
    Hold = 4,
    Strobe = 8,
}