using ThinMPm.Constants;

namespace ThinMPm.Views.Behaviors;

public class IconColorBehavior : Behavior<Image>
{
    public Color? TintColor { get; set; }

    protected override void OnAttachedTo(Image image)
    {
        base.OnAttachedTo(image);
        image.HandlerChanged += OnHandlerChanged;
        image.PropertyChanged += OnPropertyChanged;
    }

    protected override void OnDetachingFrom(Image image)
    {
        base.OnDetachingFrom(image);
        image.HandlerChanged -= OnHandlerChanged;
        image.PropertyChanged -= OnPropertyChanged;
    }

    private void OnHandlerChanged(object? sender, EventArgs e)
    {
        if (sender is Image image)
        {
            ApplyTintColor(image);
        }
    }

    private void OnPropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (e.PropertyName == nameof(Image.Source) && sender is Image image)
        {
            // Sourceが変更されたら少し遅延してTintColorを再適用
            image.Dispatcher.Dispatch(() => ApplyTintColor(image));
        }
    }

    private void ApplyTintColor(Image image)
    {
        var tintColor = TintColor ?? ColorConstants.IconColor;

#if ANDROID
        if (image.Handler?.PlatformView is Android.Widget.ImageView imageView)
        {
            var color = Android.Graphics.Color.Argb(
                (int)(tintColor.Alpha * 255),
                (int)(tintColor.Red * 255),
                (int)(tintColor.Green * 255),
                (int)(tintColor.Blue * 255));
            imageView.SetColorFilter(new Android.Graphics.PorterDuffColorFilter(color, Android.Graphics.PorterDuff.Mode.SrcIn!));
        }
#elif IOS
        if (image.Handler?.PlatformView is UIKit.UIImageView imageView && imageView.Image != null)
        {
            imageView.Image = imageView.Image.ImageWithRenderingMode(UIKit.UIImageRenderingMode.AlwaysTemplate);
            imageView.TintColor = UIKit.UIColor.FromRGBA(
                (nfloat)tintColor.Red,
                (nfloat)tintColor.Green,
                (nfloat)tintColor.Blue,
                (nfloat)tintColor.Alpha);
        }
#endif
    }
}
