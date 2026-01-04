using ThinMPm.Constants;

namespace ThinMPm.Views.Button;

public class TextButton : Label
{
    public TextButton(string text, EventHandler<TappedEventArgs> onTapped)
    {
        Text = text;
        this.SetAppThemeColor(Label.TextColorProperty, ColorConstants.PrimaryTextColorLight, ColorConstants.PrimaryTextColorDark);
        FontSize = 16;
        VerticalOptions = LayoutOptions.Center;
        HorizontalOptions = LayoutOptions.Center;

        var tap = new TapGestureRecognizer();
        tap.Tapped += onTapped;
        GestureRecognizers.Add(tap);
    }
}
