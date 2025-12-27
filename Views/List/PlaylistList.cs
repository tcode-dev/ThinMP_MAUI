using ThinMPm.Views.ListItem;

namespace ThinMPm.Views.List;

public class PlaylistList : CollectionView
{
    public PlaylistList(EventHandler<TappedEventArgs> onPlaylistTapped)
    {
        ItemTemplate = new DataTemplate(() => new PlaylistListItem(onPlaylistTapped));
    }
}
