using ThinMPm.Views.ListItem;

namespace ThinMPm.Views.List;

public class ArtistList : CollectionView
{
    public ArtistList(EventHandler<TappedEventArgs> onArtistTapped)
    {
        ItemTemplate = new DataTemplate(() => new ArtistListItem(onArtistTapped));
    }
}
