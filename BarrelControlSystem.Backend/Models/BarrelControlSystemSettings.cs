namespace BarrelControlSystem.Backend.Models;

public class BarrelControlSystemSettings
{
    public bool SimulateGpio { get; set; } = true;
    public int GpioIoPinCount { get; set; } = 26;
    public int RelayPinCount { get; set; } = 16;

    public List<RelayConfig> RelayBoardConfig { get; set; } = [];

    public override string ToString()
    {
        string relayConfigs = string.Join("\n\n", RelayBoardConfig.Select(r => r.ToString()));
        return $"Barrel Control System Settings:\n" +
               $"------------------------------\n" +
               $"Simulate GPIO: {SimulateGpio}\n" +
               $"GPIO I/O Pin Count: {GpioIoPinCount}\n" +
               $"Relay Pin Count: {RelayPinCount}\n\n" +
               $"Relay Board Configuration:\n" +
               $"{relayConfigs}";
    }
}