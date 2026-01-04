using ThinMPm.Constants;
using ThinMPm.Views.Behaviors;

namespace ThinMPm.Views.Button;

public class BaseButton : Grid
{
    protected readonly Image Icon;

    public BaseButton(string source, EventHandler<TappedEventArgs> onTapped, double iconSize = LayoutConstants.ButtonMedium)
    {
        var containerSize = Math.Max(iconSize, LayoutConstants.ButtonMedium);

        WidthRequest = containerSize;
        HeightRequest = containerSize;

        Icon = new Image
        {
            Source = source,
            WidthRequest = iconSize,
            HeightRequest = iconSize,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };

        Icon.Behaviors.Add(new IconColorBehavior());

        Children.Add(Icon);

        var tap = new TapGestureRecognizer();
        tap.Tapped += onTapped;
        GestureRecognizers.Add(tap);
    }
}
