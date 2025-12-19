using ThinMPm.Views.Behaviors;

namespace ThinMPm.Views.Button;

public class BackButton : Grid
{
    private const string IconArrowBack = "arrowback";

    public BackButton()
    {
        WidthRequest = 50;
        HeightRequest = 50;

        var icon = new Image
        {
            Source = IconArrowBack,
            WidthRequest = 25,
            HeightRequest = 25,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center
        };
        icon.Behaviors.Add(new IconColorBehavior());

        Children.Add(icon);

        var tap = new TapGestureRecognizer();
        tap.Tapped += async (s, e) => await Shell.Current.GoToAsync("..");
        GestureRecognizers.Add(tap);
    }
}
