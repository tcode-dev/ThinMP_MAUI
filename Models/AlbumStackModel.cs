using ThinMPm.Contracts.Models;

namespace ThinMPm.Models;

public class AlbumStackModel : IAlbumStackModel
{
    public AlbumStackModel(IList<IAlbumModel> albums, int columnCount)
    {
        Albums = albums;
        ColumnCount = columnCount;
    }

    public IList<IAlbumModel> Albums { get; }
    public int ColumnCount { get; }
}
