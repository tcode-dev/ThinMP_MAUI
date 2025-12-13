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
        TextColor = ColorConstants.GetTextColor();
    }
}
