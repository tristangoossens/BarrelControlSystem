using System.Device.Gpio;
using BarrelControlSystem.Backend.Handlers.GPIO;
using BarrelControlSystem.Backend.Models;
using BarrelControlSystem.Backend.Models.Enums;
using BarrelControlSystem.Backend.Exceptions;
using BarrelControlSystem.Models.Interfaces;

namespace BarrelControlSystem.Backend.Handlers;

public class ActionHandler : IDisposable
{
    private IGpioHandler? _currentHandler;
    private bool _lastSimulateSetting;

    public ActionHandler()
    {
        _lastSimulateSetting = ConfigurationHandler.Settings.SimulateGpio;
        _currentHandler = CreateHandler(_lastSimulateSetting);
    }

    private IGpioHandler GetHandler()
    {
        bool currentSimulateSetting = ConfigurationHandler.Settings.SimulateGpio;
        
        if (currentSimulateSetting != _lastSimulateSetting || _currentHandler == null)
        {
            LoggingHandler.LogInfo($"SimulateGpio setting changed from {_lastSimulateSetting} to {currentSimulateSetting}. Updating handler.");
            _currentHandler?.Dispose();
            _currentHandler = CreateHandler(currentSimulateSetting);
            _lastSimulateSetting = currentSimulateSetting;
        }

        return _currentHandler;
    }

    private IGpioHandler CreateHandler(bool simulate)
    {
        if (simulate)
        {
            return new SimulatedGpioHandler();
        }
        else
        {
            try
            {
                return new GpioHandler(new GpioController());
            }
            catch (Exception ex)
            {
                LoggingHandler.LogError($"Failed to initialize actual GpioHandler: {ex.Message}. Falling back to simulated.");
                return new SimulatedGpioHandler();
            }
        }
    }

    public void Toggle(RelayConfig relay)
    {
        CanPerformAction(relay, DeviceCapabilityType.Toggle);
        LoggingHandler.LogInfo($"Toggling device '{relay.ConnectedDevice?.Name}' on pin {relay.PinNumber}.");
        Toggle(relay.PinNumber);
        relay.LastUsed = DateTime.Now;
    }

    public async Task Pulse(RelayConfig relay, int durationMs)
    {
        CanPerformAction(relay, DeviceCapabilityType.Pulse);
        LoggingHandler.LogInfo($"Pulsing device '{relay.ConnectedDevice?.Name}' on pin {relay.PinNumber} for {durationMs}ms.");
        await Pulse(relay.PinNumber, durationMs);
        relay.LastUsed = DateTime.Now;
    }

    public async Task Strobe(RelayConfig relay, int count, int durationMs)
    {
        CanPerformAction(relay, DeviceCapabilityType.Strobe);
        LoggingHandler.LogInfo($"Strobing device '{relay.ConnectedDevice?.Name}' on pin {relay.PinNumber} (Count: {count}, Duration: {durationMs}ms).");
        await Strobe(relay.PinNumber, count, durationMs);
        relay.LastUsed = DateTime.Now;
    }

    public async Task Hold(RelayConfig relay, int durationMs)
    {
        CanPerformAction(relay, DeviceCapabilityType.Hold);
        LoggingHandler.LogInfo($"Holding device '{relay.ConnectedDevice?.Name}' on pin {relay.PinNumber} for {durationMs}ms.");
        await Hold(relay.PinNumber, durationMs);
        relay.LastUsed = DateTime.Now;
    }

    private void CanPerformAction(RelayConfig relay, DeviceCapabilityType requiredCapability)
    {
        if (relay.ConnectedDevice.Capabilities == DeviceCapabilityType.None)
        {
            var message = $"Cannot perform {requiredCapability} on {relay.ConnectedDevice.Name}: Device has no capabilities configured.";
            LoggingHandler.LogWarning(message);
            throw new InvalidCapabilityException(relay.ConnectedDevice.Name, requiredCapability.ToString());
        }

        if (!relay.ConnectedDevice.Capabilities.HasFlag(requiredCapability))
        {
            var message = $"Cannot perform {requiredCapability} on {relay.ConnectedDevice.Name}: Device does not support this action. Supported: {relay.ConnectedDevice.Capabilities}";
            LoggingHandler.LogWarning(message);
            throw new InvalidCapabilityException(relay.ConnectedDevice.Name, requiredCapability.ToString());
        }
    }

    public void Toggle(int pinNumber)
    {
        LoggingHandler.LogInfo($"Toggling pin {pinNumber}.");
        var handler = GetHandler();
        if (handler.IsPinHigh(pinNumber))
        {
            handler.SetLow(pinNumber);
        }
        else
        {
            handler.SetHigh(pinNumber);
        }
    }

    public async Task Pulse(int pinNumber, int durationMs)
    {
        LoggingHandler.LogInfo($"Pulsing pin {pinNumber} for {durationMs}ms.");
        await GetHandler().Pulse(pinNumber, durationMs);
    }

    public async Task Strobe(int pinNumber, int count, int durationMs)
    {
        LoggingHandler.LogInfo($"Strobing pin {pinNumber} (Count: {count}, Duration: {durationMs}ms).");
        var handler = GetHandler();
        for (int i = 0; i < count; i++)
        {
            handler.SetHigh(pinNumber);
            await Task.Delay(durationMs);
            handler.SetLow(pinNumber);
            if (i < count - 1)
            {
                await Task.Delay(durationMs);
            }
        }
    }

    public async Task Hold(int pinNumber, int durationMs)
    {
        LoggingHandler.LogInfo($"Holding pin {pinNumber} for {durationMs}ms.");
        var handler = GetHandler();
        handler.SetHigh(pinNumber);
        await Task.Delay(durationMs);
        handler.SetLow(pinNumber);
    }

    public void Dispose()
    {
        _currentHandler?.Dispose();
    }
}