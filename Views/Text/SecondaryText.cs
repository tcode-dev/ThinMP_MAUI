using ThinMPm.Constants;

namespace ThinMPm.Views.Text;

public class SecondaryText : BaseText
{
    public SecondaryText()
    {
        this.SetAppThemeColor(Label.TextColorProperty, ColorConstants.SecondaryTextColorLight, ColorConstants.SecondaryTextColorDark);
    }
}
