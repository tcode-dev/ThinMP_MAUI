using ThinMPm.Constants;

namespace ThinMPm.Views.Text;

public class PrimaryTitle : Label
{
    public PrimaryTitle()
    {
        HeightRequest = LayoutConstants.HeaderHeight;
        FontAttributes = FontAttributes.Bold;
        HorizontalTextAlignment = TextAlignment.Center;
        VerticalTextAlignment = TextAlignment.Center;

        var isDark = Application.Current?.RequestedTheme == AppTheme.Dark;
        TextColor = isDark ? Colors.White : Colors.Black;
    }
}
