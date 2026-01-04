using ThinMPm.Constants;

namespace ThinMPm.Views.Behaviors;

public class IconColorBehavior : Behavior<Image>
{
    public Color? TintColor { get; set; }
    private Image? _image;

    protected override void OnAttachedTo(Image image)
    {
        base.OnAttachedTo(image);
        _image = image;
        image.HandlerChanged += OnHandlerChanged;
        image.PropertyChanged += OnPropertyChanged;

        if (Application.Current != null)
        {
            Application.Current.RequestedThemeChanged += OnRequestedThemeChanged;
        }
    }

    protected override void OnDetachingFrom(Image image)
    {
        base.OnDetachingFrom(image);
        image.HandlerChanged -= OnHandlerChanged;
        image.PropertyChanged -= OnPropertyChanged;

        if (Application.Current != null)
        {
            Application.Current.RequestedThemeChanged -= OnRequestedThemeChanged;
        }

        _image = null;
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
            image.Dispatcher.Dispatch(() => ApplyTintColor(image));
        }
    }

    private void OnRequestedThemeChanged(object? sender, AppThemeChangedEventArgs e)
    {
        if (_image != null)
        {
            _image.Dispatcher.Dispatch(() => ApplyTintColor(_image));
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
