using ThinMPm.Constants;

namespace ThinMPm.Views.Text;

public class SecondaryTitle : Label
{
    public SecondaryTitle()
    {
        HorizontalTextAlignment = TextAlignment.Center;
        VerticalTextAlignment = TextAlignment.Center;
        TextColor = ColorConstants.GetTextColor();
    }
}
