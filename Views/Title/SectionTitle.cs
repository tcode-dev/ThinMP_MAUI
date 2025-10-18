namespace ThinMPm.Views.Title;

public class SectionTitle : ContentView
{
    public SectionTitle(string text)
    {
        Content = new Label()
        {
            Text = text,
            FontAttributes = FontAttributes.Bold,
            FontSize = 16,
            VerticalTextAlignment = TextAlignment.Center,
            Margin = new Thickness(20, 0)
        };
    }
}