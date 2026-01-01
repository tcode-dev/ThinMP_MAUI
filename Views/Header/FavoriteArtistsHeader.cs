using ThinMPm.Resources.Strings;
using ThinMPm.Views.Page;

namespace ThinMPm.Views.Header;

public class FavoriteArtistsHeader : ListMenuHeader
{
    protected override string EditPageRoute => nameof(FavoriteArtistsEditPage);

    public FavoriteArtistsHeader()
    {
        Title = AppResources.FavoriteArtists;
    }
}
