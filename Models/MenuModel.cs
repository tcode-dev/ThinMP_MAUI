using ThinMPm.Contracts.Models;

namespace ThinMPm.Models;

public class MenuModel(string title, string page) : IMenuModel
{
  public string Title { get; set; } = title;
  public string Page { get; set; } = page;
}