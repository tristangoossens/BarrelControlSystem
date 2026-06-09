using Avalonia;
using Avalonia.Controls;
using Avalonia.Metadata;

namespace BarrelControlSystem.UI.Views.Components;

public partial class AppHeaderView : UserControl
{
    public AppHeaderView()
    {
        InitializeComponent();
    }

    public static readonly StyledProperty<object?> HeaderContentProperty =
        AvaloniaProperty.Register<AppHeaderView, object?>(nameof(HeaderContent));

    public object? HeaderContent
    {
        get => GetValue(HeaderContentProperty);
        set => SetValue(HeaderContentProperty, value);
    }

    public static readonly StyledProperty<bool> IsHomePageProperty =
        AvaloniaProperty.Register<AppHeaderView, bool>(nameof(IsHomePage), defaultValue: true);
    
    public bool IsHomePage { get => GetValue(IsHomePageProperty); set => SetValue(IsHomePageProperty, value); }
    
    public static readonly StyledProperty<string?> IconKindProperty =
        AvaloniaProperty.Register<AppHeaderView, string?>(nameof(IconKind));
        
    public string? IconKind { get => GetValue(IconKindProperty); set => SetValue(IconKindProperty, value); }

    public static readonly StyledProperty<string?> TitleProperty =
        AvaloniaProperty.Register<AppHeaderView, string?>(nameof(Title), defaultValue: "Barrel Control system (BCS)");
        
    public string? Title { get => GetValue(TitleProperty); set => SetValue(TitleProperty, value); }

    public static readonly StyledProperty<string?> DescriptionProperty =
        AvaloniaProperty.Register<AppHeaderView, string?>(nameof(Description), defaultValue: "Control system for lights, horns and other electrical devices through a relay for the Barrel challenge");
        
    public string? Description { get => GetValue(DescriptionProperty); set => SetValue(DescriptionProperty, value); }
}