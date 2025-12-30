using CommunityToolkit.Maui.Markup;
using ThinMPm.Constants;
using ThinMPm.Contracts.Models;
using ThinMPm.Views.Page;
using ThinMPm.Views.Utils;
using ThinMPm.Views.Text;

namespace ThinMPm.Views.ListItem;

public class ArtistListItem : Grid
{
    public ArtistListItem()
    {
        var tapGesture = new TapGestureRecognizer();
        tapGesture.Tapped += OnTapped;
        GestureRecognizers.Add(tapGesture);

        Padding = new Thickness(LayoutConstants.SpacingLarge, 0, 0, 0);

        RowDefinitions.Add(new RowDefinition { Height = LayoutConstants.HeightMedium });
        RowDefinitions.Add(new RowDefinition { Height = 1 });

        Children.Add(
            new PrimaryText()
                .Bind(Label.TextProperty, nameof(IArtistModel.Name))
                .Margin(new Thickness(LayoutConstants.SpacingMedium, LayoutConstants.SpacingSmall, 0, 0))
                .CenterVertical()
                .Row(0)
                .Column(0)
        );

        Children.Add(
            new Separator()
                .Row(1)
                .ColumnSpan(2)
        );
    }

    private async void OnTapped(object? sender, TappedEventArgs e)
    {
        if (BindingContext is IArtistModel artist)
        {
            await Shell.Current.GoToAsync($"{nameof(ArtistDetailPage)}?ArtistId={artist.Id}");
        }
    }
}
