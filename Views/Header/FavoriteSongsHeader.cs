using ThinMPm.Resources.Strings;
using ThinMPm.Views.Page;

namespace ThinMPm.Views.Header;

public class FavoriteSongsHeader : ListMenuHeader
{
    protected override string EditPageRoute => nameof(FavoriteSongsEditPage);

    public FavoriteSongsHeader()
    {
        Title = AppResources.FavoriteSongs;
    }
}
