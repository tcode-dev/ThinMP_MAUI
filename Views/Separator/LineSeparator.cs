using ThinMPm.Constants;

namespace ThinMPm.Views.Separator;

public class LineSeparator : BoxView
{
    public LineSeparator()
    {
        HeightRequest = LayoutConstants.LineHeight;
        BackgroundColor = ColorConstants.GetLineColor();
    }
}
