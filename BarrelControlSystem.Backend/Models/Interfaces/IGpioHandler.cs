namespace BarrelControlSystem.Models.Interfaces;

public interface IGpioHandler : IDisposable
{
    void SetHigh(int pinNumber);
    void SetLow(int pinNumber);
    
    void SetAllLow();
    void SetAllHigh();
    
    Task Pulse(int pinNumber, int durationMs);
    bool IsPinHigh(int pinNumber);
}