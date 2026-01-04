using ThinMPm.Constants;

namespace ThinMPm.Views.Text;

public class PrimaryText : BaseText
{
    public PrimaryText()
    {
        this.SetAppThemeColor(Label.TextColorProperty, ColorConstants.PrimaryTextColorLight, ColorConstants.PrimaryTextColorDark);
    }
}
