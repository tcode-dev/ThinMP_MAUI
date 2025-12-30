using ThinMPm.Contracts.Models;

namespace ThinMPm.Models;

public class SectionTitleModel : ISectionTitleModel
{
    public SectionTitleModel(string title)
    {
        Title = title;
    }

    public string Title { get; }
}
