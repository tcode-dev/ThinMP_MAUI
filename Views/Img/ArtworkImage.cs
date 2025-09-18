
using Microsoft.Maui.Controls.Shapes;
using ThinMPm.Contracts.Services;

namespace ThinMPm.Views.Img;

public class ArtworkImage : Image
{
  public static readonly BindableProperty ImageIdProperty =
    BindableProperty.Create(
      nameof(ImageId),
      typeof(string),
      typeof(ArtworkImage),
      propertyChanged: OnImageIdChanged);

  public string ImageId
  {
    get => (string)GetValue(ImageIdProperty);
    set => SetValue(ImageIdProperty, value);
  }

  public double CornerRadius { get; set; }

  public ArtworkImage(double cornerRadius = 5)
  {
    CornerRadius = cornerRadius;
    SizeChanged += OnSizeChanged;
  }

  private void OnSizeChanged(object? sender, EventArgs e)
  {
    if (Width > 0 && Height > 0 && CornerRadius > 0)
    {
      Clip = new RoundRectangleGeometry
      {
        CornerRadius = CornerRadius,
        Rect = new Rect(0, 0, Width, Height)
      };
    }
    else
    {
      Clip = null;
    }
  }

  private static async void OnImageIdChanged(BindableObject bindable, object oldValue, object newValue)
  {
    var control = (ArtworkImage)bindable;
    var imageId = newValue as string;

    if (string.IsNullOrEmpty(imageId))
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
      var artwork = await artworkService.GetArtwork(imageId);
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