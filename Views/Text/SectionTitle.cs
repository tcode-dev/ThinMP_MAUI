using ThinMPm.Constants;

namespace ThinMPm.Views.Text;

public class SectionTitle : PrimaryText
{
    public SectionTitle(string text)
    {
        Text = text;
        FontAttributes = FontAttributes.Bold;
        FontSize = 24;
        VerticalTextAlignment = TextAlignment.Center;
        Margin = new Thickness(LayoutConstants.SpacingLarge);
    }
}
