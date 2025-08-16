using ThinMPm.Platforms.iOS.Models.Contracts;
using ThinMPm.Platforms.iOS.ValueObjects;

namespace ThinMPm.Platforms.iOS.Repositories.Contracts;

public interface IArtistRepository
{
    IList<IArtistModel> FindAll();

    IArtistModel? FindById(Id id);

    IList<IArtistModel> FindByIds(IList<Id> ids);
}
