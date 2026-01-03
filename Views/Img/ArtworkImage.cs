using FFImageLoading.Maui;
using Microsoft.Maui.Controls.Shapes;
using ThinMPm.Contracts.Services;

namespace ThinMPm.Views.Img;

public class ArtworkImage : CachedImage
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

    public static readonly BindableProperty IsCircleProperty =
        BindableProperty.Create(
            nameof(IsCircle),
            typeof(bool),
            typeof(ArtworkImage),
            false,
            propertyChanged: OnIsCircleChanged);

    public double CornerRadius { get; set; }

    public bool IsCircle
    {
        get => (bool)GetValue(IsCircleProperty);
        set => SetValue(IsCircleProperty, value);
    }

    public ArtworkImage(double cornerRadius = 5)
    {
        CornerRadius = cornerRadius;
        SizeChanged += OnSizeChanged;

        FadeAnimationEnabled = false;
        CacheType = FFImageLoading.Cache.CacheType.Memory;
        DownsampleToViewSize = true;
    }

    private static void OnIsCircleChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ArtworkImage)bindable;
        control.UpdateClip();
    }

    private void OnSizeChanged(object? sender, EventArgs e)
    {
        UpdateClip();
    }

    private void UpdateClip()
    {
        if (Width > 0 && Height > 0)
        {
            if (IsCircle)
            {
                Clip = new EllipseGeometry
                {
                    Center = new Point(Width / 2, Height / 2),
                    RadiusX = Width / 2,
                    RadiusY = Height / 2
                };
            }
            else if (CornerRadius > 0)
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
    }

    private static async void OnImageIdChanged(BindableObject bindable, object oldValue, object newValue)
    {
        var control = (ArtworkImage)bindable;
        var imageId = newValue as string;

        if (string.IsNullOrEmpty(imageId))
        {
            control.Source = "song.png";
            return;
        }

        var artworkService = Application.Current!.Handler!.MauiContext!.Services.GetRequiredService<IArtworkService>();

        try
        {
            var artwork = await artworkService.GetArtwork(imageId);

            if (artwork != null)
            {
                control.Source = ImageSource.FromStream(() => new MemoryStream(artwork));
            }
            else
            {
                control.Source = "song.png";
            }
        }
        catch
        {
            control.Source = "song.png";
        }
    }
}
