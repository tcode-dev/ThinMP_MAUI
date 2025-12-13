using ThinMPm.Constants;

namespace ThinMPm.Views.Text;

public class PrimaryText : Label
{
    public PrimaryText()
    {
        TextColor = ColorConstants.GetPrimaryTextColor();
    }
}
