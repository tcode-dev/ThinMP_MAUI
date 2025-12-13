using ThinMPm.Constants;

namespace ThinMPm.Views.Text;

public class PrimaryTitle : PrimaryText
{
    public PrimaryTitle()
    {
        HeightRequest = LayoutConstants.HeaderHeight;
        FontAttributes = FontAttributes.Bold;
        HorizontalTextAlignment = TextAlignment.Center;
        VerticalTextAlignment = TextAlignment.Center;
    }
}
