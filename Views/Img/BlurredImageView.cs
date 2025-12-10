namespace ThinMPm.Views.Img;

public class BlurredImageView : View
{
    public static readonly BindableProperty ImageIdProperty =
        BindableProperty.Create(
            nameof(ImageId),
            typeof(string),
            typeof(BlurredImageView),
            default(string));

    public string ImageId
    {
        get => (string)GetValue(ImageIdProperty);
        set => SetValue(ImageIdProperty, value);
    }

    public static readonly BindableProperty BlurRadiusProperty =
        BindableProperty.Create(
            nameof(BlurRadius),
            typeof(float),
            typeof(BlurredImageView),
            20f);

    public float BlurRadius
    {
        get => (float)GetValue(BlurRadiusProperty);
        set => SetValue(BlurRadiusProperty, value);
    }
}
