using ThinMPm.Contracts.Models;
using ThinMPm.Views.ListItem;

namespace ThinMPm.Views.Selector;

public class ArtistDetailTemplateSelector : DataTemplateSelector
{
    private readonly DataTemplate _sectionTitleTemplate;
    private readonly DataTemplate _albumStackTemplate;
    private readonly DataTemplate _songTemplate;

    public ArtistDetailTemplateSelector(EventHandler<TappedEventArgs> songTappedHandler)
    {
        _sectionTitleTemplate = new DataTemplate(() => new SectionTitleListItem());
        _albumStackTemplate = new DataTemplate(() => new AlbumStackListItem());
        _songTemplate = new DataTemplate(() => new SongListItem(songTappedHandler));
    }

    protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
    {
        return item switch
        {
            ISectionTitleModel => _sectionTitleTemplate,
            IAlbumStackModel => _albumStackTemplate,
            ISongModel => _songTemplate,
            _ => _songTemplate
        };
    }
}
