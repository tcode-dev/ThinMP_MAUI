using CoreGraphics;
using Microsoft.Maui.Handlers;
using ThinMPm.Views.Background;
using UIKit;

namespace ThinMPm.Platforms.iOS.Handlers;

public class BlurBackgroundViewHandler : ViewHandler<BlurBackgroundView, UIVisualEffectView>
{
    public static IPropertyMapper<BlurBackgroundView, BlurBackgroundViewHandler> PropertyMapper =
        new PropertyMapper<BlurBackgroundView, BlurBackgroundViewHandler>(ViewHandler.ViewMapper);

    public BlurBackgroundViewHandler() : base(PropertyMapper)
    {
    }

    protected override UIVisualEffectView CreatePlatformView()
    {
        var blurEffect = UIBlurEffect.FromStyle(UIBlurEffectStyle.SystemMaterial);
        var view = new UIVisualEffectView(blurEffect);
        return view;
    }

    public override void PlatformArrange(Rect frame)
    {
        base.PlatformArrange(frame);
        PlatformView.Frame = new CGRect(frame.X, frame.Y, frame.Width, frame.Height);
    }
}
