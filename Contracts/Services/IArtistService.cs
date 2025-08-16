using ThinMPm.Contracts.Models;

namespace ThinMPm.Contracts.Services;

public interface IArtistService
{
    IList<IArtistModel> FindAll();

    IArtistModel? FindById(string id);

    IList<IArtistModel> FindByIds(IList<string> ids);
}