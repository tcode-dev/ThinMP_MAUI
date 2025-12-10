using Microsoft.Maui.Handlers;
using ThinMPm.Views.Img;
using ThinMPm.Contracts.Services;
using UIKit;
using CoreGraphics;

namespace ThinMPm.Platforms.iOS.Handlers;

public class BlurredImageViewHandler : ViewHandler<BlurredImageView, UIView>
{
    public static IPropertyMapper<BlurredImageView, BlurredImageViewHandler> PropertyMapper = new PropertyMapper<BlurredImageView, BlurredImageViewHandler>(ViewHandler.ViewMapper)
    {
        [nameof(BlurredImageView.ImageId)] = MapImageId,
        [nameof(BlurredImageView.BlurRadius)] = MapBlurRadius
    };

    private UIImageView? imageView;
    private UIVisualEffectView? blurView;

    public BlurredImageViewHandler() : base(PropertyMapper)
    {
    }

    protected override UIView CreatePlatformView()
    {
        var containerView = new UIView();

        imageView = new UIImageView
        {
            ContentMode = UIViewContentMode.ScaleAspectFill,
            ClipsToBounds = true
        };

        var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.Regular);
        blurView = new UIVisualEffectView(blurEffect);

        containerView.AddSubview(imageView);
        containerView.AddSubview(blurView);

        return containerView;
    }

    protected override void ConnectHandler(UIView platformView)
    {
        base.ConnectHandler(platformView);
        UpdateImage();
    }

    protected override void DisconnectHandler(UIView platformView)
    {
        base.DisconnectHandler(platformView);
        imageView?.Dispose();
        blurView?.Dispose();
        imageView = null;
        blurView = null;
    }

    public static void MapImageId(BlurredImageViewHandler handler, BlurredImageView view)
    {
        handler.UpdateImage();
    }

    public static void MapBlurRadius(BlurredImageViewHandler handler, BlurredImageView view)
    {
        // UIVisualEffectViewはblur radiusを直接設定できないため、
        // ここでは異なるblur styleを選択することで対応
        if (handler.blurView != null)
        {
            UIBlurEffectStyle style = view.BlurRadius switch
            {
                < 10 => UIBlurEffectStyle.Light,
                < 20 => UIBlurEffectStyle.Regular,
                _ => UIBlurEffectStyle.Dark
            };
            var blurEffect = UIBlurEffect.FromStyle(style);
            handler.blurView.Effect = blurEffect;
        }
    }

    private async void UpdateImage()
    {
        if (imageView == null || VirtualView == null)
            return;

        var imageId = VirtualView.ImageId;
        if (string.IsNullOrEmpty(imageId))
        {
            imageView.Image = null;
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
                using var data = Foundation.NSData.FromArray(artwork);
                imageView.Image = UIImage.LoadFromData(data);
            }
            else
            {
                imageView.Image = null;
            }
        }
        catch
        {
            imageView.Image = null;
        }
    }

    private void UpdateLayout()
    {
        if (PlatformView == null || imageView == null || blurView == null)
            return;

        var bounds = PlatformView.Bounds;
        imageView.Frame = bounds;
        blurView.Frame = bounds;
    }

    public override void PlatformArrange(Rect frame)
    {
        base.PlatformArrange(frame);
        UpdateLayout();
    }
}
