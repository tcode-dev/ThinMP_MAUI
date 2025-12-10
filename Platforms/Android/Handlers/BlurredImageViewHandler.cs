using Android.Content;
using Android.Graphics;
using Android.Renderscripts;
using Android.Views;
using Android.Widget;
using Microsoft.Maui.Handlers;
using ThinMPm.Views.Img;
using ThinMPm.Contracts.Services;
using System.IO;
using RSElement = Android.Renderscripts.Element;

namespace ThinMPm.Platforms.Android.Handlers;

public class BlurredImageViewHandler : ViewHandler<BlurredImageView, FrameLayout>
{
    public static IPropertyMapper<BlurredImageView, BlurredImageViewHandler> PropertyMapper = new PropertyMapper<BlurredImageView, BlurredImageViewHandler>(ViewHandler.ViewMapper)
    {
        [nameof(BlurredImageView.ImageId)] = MapImageId,
        [nameof(BlurredImageView.BlurRadius)] = MapBlurRadius
    };

    private ImageView? blurredImageView;

    public BlurredImageViewHandler() : base(PropertyMapper)
    {
    }

    protected override FrameLayout CreatePlatformView()
    {
        var context = Context;
        var frameLayout = new FrameLayout(context);

        blurredImageView = new ImageView(context);
        blurredImageView.SetScaleType(ImageView.ScaleType.CenterCrop);
        blurredImageView.SetMinimumWidth(0);
        blurredImageView.SetMinimumHeight(0);

        frameLayout.AddView(blurredImageView, new FrameLayout.LayoutParams(
            ViewGroup.LayoutParams.MatchParent,
            ViewGroup.LayoutParams.MatchParent));

        return frameLayout;
    }

    protected override void ConnectHandler(FrameLayout platformView)
    {
        base.ConnectHandler(platformView);
        UpdateImage();
    }

    protected override void DisconnectHandler(FrameLayout platformView)
    {
        base.DisconnectHandler(platformView);
        blurredImageView?.SetImageBitmap(null);
        blurredImageView?.Dispose();
        blurredImageView = null;
    }

    public static void MapImageId(BlurredImageViewHandler handler, BlurredImageView view)
    {
        handler.UpdateImage();
    }

    public static void MapBlurRadius(BlurredImageViewHandler handler, BlurredImageView view)
    {
        handler.UpdateImage();
    }

    private async void UpdateImage()
    {
        if (blurredImageView == null || VirtualView == null)
            return;

        var imageId = VirtualView.ImageId;
        if (string.IsNullOrEmpty(imageId))
        {
            blurredImageView.SetImageBitmap(null);
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
                    var blurredBitmap = BlurBitmap(bitmap, VirtualView.BlurRadius);
                    blurredImageView.SetImageBitmap(blurredBitmap);
                }
            }
            else
            {
                blurredImageView.SetImageBitmap(null);
            }
        }
        catch
        {
            blurredImageView.SetImageBitmap(null);
        }
    }

    private Bitmap? BlurBitmap(Bitmap originalBitmap, float radius)
    {
        try
        {
            // スケールダウンしてぼかし処理を高速化
            var scaleFactor = 0.25f;
            var width = (int)(originalBitmap.Width * scaleFactor);
            var height = (int)(originalBitmap.Height * scaleFactor);

            var inputBitmap = Bitmap.CreateScaledBitmap(originalBitmap, width, height, false);
            var outputBitmap = Bitmap.CreateBitmap(inputBitmap);

            if (Context == null)
                return outputBitmap;

#pragma warning disable CA1422
            var renderScript = RenderScript.Create(Context);
            var blurScript = ScriptIntrinsicBlur.Create(renderScript, RSElement.U8_4(renderScript));
#pragma warning restore CA1422

            var tmpIn = Allocation.CreateFromBitmap(renderScript, inputBitmap);
            var tmpOut = Allocation.CreateFromBitmap(renderScript, outputBitmap);

            blurScript.SetRadius(Math.Min(25f, radius));
            blurScript.SetInput(tmpIn);
            blurScript.ForEach(tmpOut);

            tmpOut.CopyTo(outputBitmap);

            // クリーンアップ
            tmpIn.Dispose();
            tmpOut.Dispose();
            blurScript.Dispose();
            renderScript.Dispose();
            inputBitmap.Dispose();

            // 元のサイズにスケールアップ
            var finalBitmap = Bitmap.CreateScaledBitmap(
                outputBitmap,
                originalBitmap.Width,
                originalBitmap.Height,
                true);

            outputBitmap.Dispose();

            return finalBitmap;
        }
        catch
        {
            return originalBitmap;
        }
    }
}
