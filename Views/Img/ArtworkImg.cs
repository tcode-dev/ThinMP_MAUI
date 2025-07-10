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

  public static readonly BindableProperty Base64SourceProperty =
      BindableProperty.Create(
          nameof(Base64Source),
          typeof(string),
          typeof(ArtworkImg),
          propertyChanged: OnBase64SourceChanged);

  public string? Base64Source
  {
    get => (string?)GetValue(Base64SourceProperty);
    set => SetValue(Base64SourceProperty, value);
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
        var base64 = await artworkService.GetArtwork(id);

        control.Base64Source = base64;
      }
      else
      {
        control.Base64Source = null;
      }
    }
    else
    {
      control.Base64Source = null;
    }
  }

  private static void OnBase64SourceChanged(BindableObject bindable, object oldValue, object newValue)
  {
    var control = (ArtworkImg)bindable;
    var base64String = newValue as string;

    if (!string.IsNullOrEmpty(base64String))
    {
      try
      {
        byte[] imageBytes = Convert.FromBase64String(base64String);
        control.Source = ImageSource.FromStream(() => new MemoryStream(imageBytes));
      }
      catch
      {
        control.Source = null;
      }
    }
    else
    {
      control.Source = null;
    }
  }
}