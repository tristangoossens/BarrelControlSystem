using Avalonia;
using Avalonia.Controls;

namespace BarrelControlSystem.UI.Views.Components;

public partial class ToastView : UserControl
{
    public ToastView()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<string?> MessageProperty =
        AvaloniaProperty.Register<ToastView, string?>(nameof(Message));

    public string? Message
    {
        get => GetValue(MessageProperty);
        set => SetValue(MessageProperty, value);
    }
}
