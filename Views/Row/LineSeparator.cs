using ThinMPm.Constants;

namespace ThinMPm.Views.Row;

public class LineSeparator : BoxView
{
    public LineSeparator()
    {
        HeightRequest = LayoutConstants.LineHeight;
        BackgroundColor = ColorConstants.GetLineColor();
    }
}
