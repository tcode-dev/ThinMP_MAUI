namespace ThinMPm.Contracts.Models;

public interface IAlbumStackModel
{
    IList<IAlbumModel> Albums { get; }
    int ColumnCount { get; }
}
