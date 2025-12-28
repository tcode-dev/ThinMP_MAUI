using ThinMPm.Views.ListItem;

namespace ThinMPm.Views.List;

public class ArtistList : CollectionView
{
    public ArtistList()
    {
        ItemTemplate = new DataTemplate(() => new ArtistListItem());
    }
}
