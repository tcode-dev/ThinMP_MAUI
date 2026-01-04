using ThinMPm.Constants;

namespace ThinMPm.Views.Utils;

public class Separator : BoxView
{
    public Separator()
    {
        HeightRequest = 1;
        this.SetAppThemeColor(BoxView.ColorProperty, ColorConstants.LineColorLight, ColorConstants.LineColorDark);
        HorizontalOptions = LayoutOptions.Fill;
    }
}
