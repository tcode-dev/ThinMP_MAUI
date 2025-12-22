using ThinMPm.Contracts.Services;
using ThinMPm.Views.ListItem;

namespace ThinMPm.Views.List;

public class ArtistList : CollectionView
{
    public ArtistList(EventHandler<TappedEventArgs> onArtistTapped, IFavoriteArtistService favoriteArtistService)
    {
        ItemTemplate = new DataTemplate(() => new ArtistListItem(onArtistTapped, favoriteArtistService));
    }
}
