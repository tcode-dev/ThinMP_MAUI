using ThinMPm.Contracts.Services;

namespace ThinMPm.Views.Img;

public class ArtworkImg : Image
{
  public static readonly BindableProperty IdProperty =
      BindableProperty.Create(
          nameof(Id),
          typeof(string),
          typeof(ArtworkImg),
          propertyChanged: OnIdChanged);

  public string Id
  {
    get => (string)GetValue(IdProperty);
    set => SetValue(IdProperty, value);
  }

  private static async void OnIdChanged(BindableObject bindable, object oldValue, object newValue)
  {
    var control = (ArtworkImg)bindable;
    var id = newValue as string;

    if (string.IsNullOrEmpty(id))
    {
      return;
    }

    var artworkService = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<IArtworkService>();

    if (artworkService == null)
    {
      return;
    }

    try
    {
      var artwork = await artworkService.GetArtwork(id);
      control.Source = artwork != null
          ? ImageSource.FromStream(() => new MemoryStream(artwork))
          : null;
    }
    catch
    {
      control.Source = null;
    }
  }
}