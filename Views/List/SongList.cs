using ThinMPm.Views.ListItem;

namespace ThinMPm.Views.List;

public class SongList : CollectionView
{
    public SongList(EventHandler<TappedEventArgs> onSongTapped)
    {
        ItemTemplate = new DataTemplate(() => new SongListItem(onSongTapped));
    }
}
