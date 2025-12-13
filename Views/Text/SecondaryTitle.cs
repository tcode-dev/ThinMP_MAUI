namespace ThinMPm.Views.Text;

public class SecondaryTitle : Label
{
    public SecondaryTitle()
    {
        HorizontalTextAlignment = TextAlignment.Center;
        VerticalTextAlignment = TextAlignment.Center;

        var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
        TextColor = isDark ? Colors.White : Colors.Black;
    }
}
