using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.Row;

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
                .Row(0).Column(1)
                .Margin(new Thickness(10, 5, 0, 0))
                .CenterVertical()
        );

        Children.Add(
            new BoxView
            {
                HeightRequest = 1,
                BackgroundColor = ColorConstants.GetLineColor()
            }
            .Row(2).ColumnSpan(2)
        );
    }
}