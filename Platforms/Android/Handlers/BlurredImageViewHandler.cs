using Android.Graphics;
using Android.Renderscripts;
using Android.Widget;
using Microsoft.Maui.Handlers;
using ThinMPm.Views.Img;
using ThinMPm.Contracts.Services;
using RSElement = Android.Renderscripts.Element;

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
                    var blurredBitmap = BlurBitmap(bitmap, VirtualView.BlurRadius);
                    PlatformView.SetImageBitmap(blurredBitmap);
                }
            }
            else
            {
                PlatformView.SetImageBitmap(null);
            }
        }
        catch
        {
            PlatformView.SetImageBitmap(null);
        }
    }

    // ビットマップにぼかし処理を適用するメソッド(Android RenderScript を使用)
    private Bitmap? BlurBitmap(Bitmap originalBitmap, float radius)
    {
        try
        {
            // スケールダウンしてぼかし処理を高速化
            var scaleFactor = 0.25f;  // 元のサイズの 25% に縮小
            var width = (int)(originalBitmap.Width * scaleFactor);
            var height = (int)(originalBitmap.Height * scaleFactor);

            // 元のビットマップを縮小したビットマップを作成(false = フィルタリングなし)
            var inputBitmap = Bitmap.CreateScaledBitmap(originalBitmap, width, height, false);
            // 出力用ビットマップを入力ビットマップのコピーとして作成
            var outputBitmap = Bitmap.CreateBitmap(inputBitmap);

            // コンテキストが null の場合は縮小ビットマップをそのまま返す
            if (Context == null)
                return outputBitmap;

            // RenderScript は非推奨(CA1422)だが、現時点で最も高速なぼかし処理手段なので警告を抑制
#pragma warning disable CA1422
            // RenderScript エンジンを作成
            var renderScript = RenderScript.Create(Context);
            // 組み込みのガウシアンぼかしスクリプトを作成(U8_4 は RGBA の 8bit x 4ch 形式)
            var blurScript = ScriptIntrinsicBlur.Create(renderScript, RSElement.U8_4(renderScript));
#pragma warning restore CA1422

            // 入力ビットマップ用のメモリ領域を RenderScript に割り当て
            var tmpIn = Allocation.CreateFromBitmap(renderScript, inputBitmap);
            // 出力ビットマップ用のメモリ領域を RenderScript に割り当て
            var tmpOut = Allocation.CreateFromBitmap(renderScript, outputBitmap);

            // ぼかし半径を設定(RenderScript の最大値は 25 なので制限)
            blurScript.SetRadius(Math.Min(25f, radius));
            // 入力データを設定
            blurScript.SetInput(tmpIn);
            // ぼかし処理を実行(各ピクセルに対して処理を適用)
            blurScript.ForEach(tmpOut);

            // 処理結果を出力ビットマップにコピー
            tmpOut.CopyTo(outputBitmap);

            // クリーンアップ: メモリリークを防ぐため使用したリソースを解放
            tmpIn.Dispose();
            tmpOut.Dispose();
            blurScript.Dispose();
            renderScript.Dispose();
            inputBitmap.Dispose();

            // 元のサイズにスケールアップ(true = バイリニアフィルタリング有効)
            var finalBitmap = Bitmap.CreateScaledBitmap(
                outputBitmap,
                originalBitmap.Width,
                originalBitmap.Height,
                true);

            // 中間ビットマップを破棄
            outputBitmap.Dispose();

            // 最終的なぼかし処理済みビットマップを返す
            return finalBitmap;
        }
        catch
        {
            // エラーが発生した場合は元のビットマップをそのまま返す
            return originalBitmap;
        }
    }
}
