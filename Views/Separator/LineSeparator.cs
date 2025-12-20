using ThinMPm.Constants;

namespace ThinMPm.Views.Separator;

public class LineSeparator : BoxView
{
    public LineSeparator()
    {
        HeightRequest = 1;
        BackgroundColor = ColorConstants.LineColor;
    }
}
