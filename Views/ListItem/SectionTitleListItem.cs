using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class SectionTitleListItem : ContentView
{
    public SectionTitleListItem()
    {
        Content = new PrimaryText
        {
            FontAttributes = FontAttributes.Bold,
            FontSize = 24,
            VerticalTextAlignment = TextAlignment.Center,
            Margin = new Thickness(LayoutConstants.SpacingLarge)
        }.Bind(Label.TextProperty, nameof(ISectionTitleModel.Title));
    }
}
