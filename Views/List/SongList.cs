using ThinMPm.Views.Row;

namespace ThinMPm.Views.List;

public class SongList : CollectionView
{
    public SongList(EventHandler<TappedEventArgs> onSongTapped)
    {
        ItemTemplate = new DataTemplate(() => new SongListItem(onSongTapped));
    }
}
