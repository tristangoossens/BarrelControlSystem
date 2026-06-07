using BarrelControlSystem.Models.Interfaces;

namespace BarrelControlSystem.Backend.Handlers.GPIO;

public class SimulatedGpioHandler : IGpioHandler
{
    private readonly Dictionary<int, bool> _pinStates = new();

    public void SetHigh(int pinNumber)
    {
        _pinStates[pinNumber] = true;
        LoggingHandler.LogInfo($"[SIMULATED] Pin {pinNumber} set to HIGH");
    }

    public void SetLow(int pinNumber)
    {
        _pinStates[pinNumber] = false;
        LoggingHandler.LogInfo($"[SIMULATED] Pin {pinNumber} set to LOW");
    }

    public void SetAllLow()
    {
        foreach (var pin in _pinStates.Keys.ToList())
        {
            _pinStates[pin] = false;
        }
        LoggingHandler.LogInfo("[SIMULATED] Setting all pins to LOW");
    }

    public void SetAllHigh()
    {
        foreach (var pin in _pinStates.Keys.ToList())
        {
            _pinStates[pin] = true;
        }
        LoggingHandler.LogInfo("[SIMULATED] Setting all pins to HIGH");
    }

    public async Task Pulse(int pinNumber, int durationMs)
    {
        LoggingHandler.LogInfo($"[SIMULATED] Pulsing Pin {pinNumber} for {durationMs}ms");
        SetHigh(pinNumber);
        await Task.Delay(durationMs);
        SetLow(pinNumber);
    }

    public bool IsPinHigh(int pinNumber)
    {
        return _pinStates.TryGetValue(pinNumber, out bool isHigh) && isHigh;
    }
    
    public void Dispose()
    {
        LoggingHandler.LogInfo("[SIMULATED] GPIO Handler disposed");
    }
}