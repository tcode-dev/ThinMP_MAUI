using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class TextButton : Label
{
    public TextButton(string text, EventHandler<TappedEventArgs> onTapped)
    {
        Text = text;
        TextColor = ColorConstants.PrimaryTextColor;
        FontSize = 16;
        VerticalOptions = LayoutOptions.Center;
        HorizontalOptions = LayoutOptions.Center;

        var tap = new TapGestureRecognizer();
        tap.Tapped += onTapped;
        GestureRecognizers.Add(tap);
    }
}
