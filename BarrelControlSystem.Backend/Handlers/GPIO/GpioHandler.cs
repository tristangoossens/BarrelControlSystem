using System.Device.Gpio;
using BarrelControlSystem.Models.Interfaces;

namespace BarrelControlSystem.Backend.Handlers.GPIO;

public class GpioHandler(GpioController controller) : IGpioHandler
{
    private readonly List<int> _openPins = new();

    public void SetHigh(int pinNumber)
    {
        OpenPinIfNotOpen(pinNumber);
        controller.Write(pinNumber, PinValue.High);
    }

    public void SetLow(int pinNumber)
    {
        OpenPinIfNotOpen(pinNumber);
        controller.Write(pinNumber, PinValue.Low);
    }

    public void SetAllLow()
    {
        foreach (var pin in _openPins)
        {
            controller.Write(pin, PinValue.Low);
        }
    }

    public void SetAllHigh()
    {
        foreach (var pin in _openPins)
        {
            controller.Write(pin, PinValue.High);
        }
    }

    public async Task Pulse(int pinNumber, int durationMs = 250)
    {
        SetHigh(pinNumber);
        await Task.Delay(TimeSpan.FromMilliseconds(durationMs));
        SetLow(pinNumber);
    }

    public bool IsPinHigh(int pinNumber)
    {
        OpenPinIfNotOpen(pinNumber);
        return controller.Read(pinNumber) == PinValue.High;
    }

    private void OpenPinIfNotOpen(int pinNumber)
    {
        if (!controller.IsPinOpen(pinNumber))
        {
            controller.OpenPin(pinNumber, PinMode.Output);
            _openPins.Add(pinNumber);
        }
        else if (controller.GetPinMode(pinNumber) != PinMode.Output)
        {
            controller.SetPinMode(pinNumber, PinMode.Output);
        }

        if (!_openPins.Contains(pinNumber))
        {
            _openPins.Add(pinNumber);
        }
    }

    public void Dispose()
    {
        SetAllLow();
        controller.Dispose();
    }
}