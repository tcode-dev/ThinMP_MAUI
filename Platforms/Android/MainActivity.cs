using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.Content;

namespace ThinMPm;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // スマートフォンの場合は portrait のみ、タブレットの場合は全方向対応
        if (DeviceInfo.Idiom != DeviceIdiom.Tablet)
        {
            RequestedOrientation = ScreenOrientation.Portrait;
        }

        // レイアウトを画面全体に拡張
        Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);

        // ステータスバーを透過
        Window.SetStatusBarColor(new Android.Graphics.Color(ContextCompat.GetColor(this, Android.Resource.Color.Transparent)));
    }
}
