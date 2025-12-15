using ThinMPm.Constants;

namespace ThinMPm.Views.Text;

public class PrimaryTitle : PrimaryText
{
    public PrimaryTitle()
    {
        HeightRequest = LayoutConstants.HeightMedium;
        FontAttributes = FontAttributes.Bold;
        HorizontalTextAlignment = TextAlignment.Center;
        VerticalTextAlignment = TextAlignment.Center;
    }
}
