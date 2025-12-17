using Android.Graphics;
using Android.Widget;
using Microsoft.Maui.Handlers;
using ThinMPm.Views.Img;
using ThinMPm.Contracts.Services;

namespace ThinMPm.Platforms.Android.Handlers;

public class BlurredImageViewHandler : ViewHandler<BlurredImageView, ImageView>
{
    public static IPropertyMapper<BlurredImageView, BlurredImageViewHandler> PropertyMapper = new PropertyMapper<BlurredImageView, BlurredImageViewHandler>(ViewHandler.ViewMapper)
    {
        [nameof(BlurredImageView.ImageId)] = MapImageId
    };

    public BlurredImageViewHandler() : base(PropertyMapper)
    {
    }

    protected override ImageView CreatePlatformView()
    {
        var context = Context;
        var imageView = new ImageView(context);

        imageView.SetScaleType(ImageView.ScaleType.CenterCrop);
        imageView.SetMinimumWidth(0);
        imageView.SetMinimumHeight(0);

        return imageView;
    }

    protected override void ConnectHandler(ImageView platformView)
    {
        base.ConnectHandler(platformView);
    }

    protected override void DisconnectHandler(ImageView platformView)
    {
        base.DisconnectHandler(platformView);
        platformView?.SetImageBitmap(null);
    }

    public static void MapImageId(BlurredImageViewHandler handler, BlurredImageView view)
    {
        handler.UpdateImage();
    }

    private async void UpdateImage()
    {
        if (PlatformView == null || VirtualView == null)
            return;

        var imageId = VirtualView.ImageId;
        if (string.IsNullOrEmpty(imageId))
        {
            PlatformView.SetImageBitmap(null);
            return;
        }

        var artworkService = MauiContext?.Services.GetRequiredService<IArtworkService>();
        if (artworkService == null)
            return;

        try
        {
            var artwork = await artworkService.GetArtwork(imageId);
            if (artwork != null)
            {
                using var bitmap = BitmapFactory.DecodeByteArray(artwork, 0, artwork.Length);
                if (bitmap != null)
                {
                    PlatformView.SetImageBitmap(bitmap);
                    ApplyBlurEffect(VirtualView.BlurRadius);
                }
            }
            else
            {
                PlatformView.SetImageBitmap(null);
                PlatformView.SetRenderEffect(null);
            }
        }
        catch
        {
            PlatformView.SetImageBitmap(null);
            PlatformView.SetRenderEffect(null);
        }
    }

    // RenderEffect を使用して View にぼかし効果を適用
    private void ApplyBlurEffect(float radius)
    {
        if (PlatformView == null)
            return;

        // RenderEffect のぼかし半径は 0 より大きい必要がある
        var blurRadius = Math.Max(1f, radius);
        var blurEffect = global::Android.Graphics.RenderEffect.CreateBlurEffect(
            blurRadius,
            blurRadius,
            Shader.TileMode.Mirror!);
        PlatformView.SetRenderEffect(blurEffect);
    }
}
