using ThinMPm.Platforms.Android.Models.Contracts;

namespace ThinMPm.Platforms.Android.Repositories.Contracts;

public interface IArtistRepository
{
    IList<IArtistModel> FindAll();

    IArtistModel? FindById(string id);

    IList<IArtistModel> FindByIds(IList<string> ids);
}