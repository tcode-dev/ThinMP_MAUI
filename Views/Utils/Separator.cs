using ThinMPm.Constants;

namespace ThinMPm.Views.Utils;

public class Separator : BoxView
{
    public Separator()
    {
        HeightRequest = 1;
        Color = ColorConstants.LineColor;
        HorizontalOptions = LayoutOptions.Fill;
    }
}
