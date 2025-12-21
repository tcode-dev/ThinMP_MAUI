using ThinMPm.Contracts.Services;
using ThinMPm.Views.ListItem;

namespace ThinMPm.Views.List;

public class SongList : CollectionView
{
    public SongList(EventHandler<TappedEventArgs> onSongTapped, IFavoriteSongService favoriteSongService)
    {
        ItemTemplate = new DataTemplate(() => new SongListItem(onSongTapped, favoriteSongService));
    }
}
