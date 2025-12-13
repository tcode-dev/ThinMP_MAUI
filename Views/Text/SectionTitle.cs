using ThinMPm.Constants;

namespace ThinMPm.Views.Text;

public class SectionTitle : ContentView
{
    public SectionTitle(string text)
    {
        Content = new Label()
        {
            Text = text,
            FontAttributes = FontAttributes.Bold,
            FontSize = 24,
            VerticalTextAlignment = TextAlignment.Center,
            Margin = new Thickness(LayoutConstants.SpacingLarge)
        };
    }
}
