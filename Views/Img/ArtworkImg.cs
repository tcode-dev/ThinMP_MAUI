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

  public string? Id
  {
    get => (string?)GetValue(IdProperty);
    set => SetValue(IdProperty, value);
  }

  public static readonly BindableProperty ArtworkProperty =
      BindableProperty.Create(
          nameof(Artwork),
          typeof(byte[]),
          typeof(ArtworkImg),
          propertyChanged: OnArtworkChanged);

  public byte[]? Artwork
  {
    get => (byte[])GetValue(ArtworkProperty);
    set => SetValue(ArtworkProperty, value);
  }

  private static async void OnIdChanged(BindableObject bindable, object oldValue, object newValue)
  {
    var control = (ArtworkImg)bindable;
    var id = newValue as string;

    if (!string.IsNullOrEmpty(id))
    {
      var artworkService = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<IArtworkService>();

      if (artworkService != null)
      {
        control.Artwork = await artworkService.GetArtwork(id);
      }
      else
      {
        control.Artwork = null;
      }
    }
    else
    {
      control.Artwork = null;
    }
  }

  private static void OnArtworkChanged(BindableObject bindable, object oldValue, object newValue)
  {
    var control = (ArtworkImg)bindable;

    try
    {
      control.Source = newValue is byte[] image ? ImageSource.FromStream(() => new MemoryStream(image)) : null;
    }
    catch
    {
      control.Source = null;
    }
  }
}