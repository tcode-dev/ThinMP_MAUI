using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Separator;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class ArtistListItem : Grid
{
    public ArtistListItem(EventHandler<TappedEventArgs> tappedHandler)
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += tappedHandler;
        GestureRecognizers.Add(tapGesture);

        Padding = new Thickness(LayoutConstants.LeadingMargin, 0, 0, 0);

        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.RowHeight });
        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.LineHeight });

        Children.Add(
            new PrimaryText()
                .Bind(Label.TextProperty, nameof(IArtistModel.Name))
                .Margin(new Thickness(LayoutConstants.SpacingMedium, LayoutConstants.SpacingSmall, 0, 0))
                .CenterVertical()
                .Row(0)
                .Column(1)
        );

        Children.Add(
            new LineSeparator()
                .Row(1)
                .ColumnSpan(2)
        );
    }
}
