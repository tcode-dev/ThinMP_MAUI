using ThinMPm.Resources.Strings;
using ThinMPm.Views.Page;

namespace ThinMPm.Views.Header;

public class PlaylistsHeader : ListMenuHeader
{
    protected override string EditPageRoute => nameof(PlaylistsEditPage);

    public PlaylistsHeader()
    {
        Title = AppResources.Playlists;
    }
}
