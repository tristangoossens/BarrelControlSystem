namespace BarrelControlSystem.Backend.Exceptions;

public class BarrelControlSystemException : Exception
{
    public BarrelControlSystemException(string message) : base(message) { }
}

public class InvalidCapabilityException : BarrelControlSystemException
{
    public InvalidCapabilityException(string deviceName, string action) 
        : base($"Device '{deviceName}' does not support the action: {action}.") { }
}
