namespace ThinMPm.Views.Text;

public class PrimaryTitle : Label
{
    private const double Height = 50;

    public PrimaryTitle()
    {
        HeightRequest = Height;
        FontAttributes = FontAttributes.Bold;
        HorizontalTextAlignment = TextAlignment.Center;
        VerticalTextAlignment = TextAlignment.Center;

        var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
        TextColor = isDark ? Colors.White : Colors.Black;
    }
}
