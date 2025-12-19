using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Views;
using AndroidX.Core.Content;
// using AndroidX.Core.View;
// using Microsoft.Maui.Controls;

namespace ThinMPm;

[Activity(Theme = "@style/Maui.SplashTheme", MainLauncher = true, LaunchMode = LaunchMode.SingleTop, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize | ConfigChanges.Density)]
public class MainActivity : MauiAppCompatActivity
{
    protected override void OnCreate(Bundle savedInstanceState)
    {
        base.OnCreate(savedInstanceState);

        // Window.SetStatusBarColor(Android.Graphics.Color.Transparent);
        // Window.DecorView.SystemUiVisibility = (StatusBarVisibility)(SystemUiFlags.LayoutStable | SystemUiFlags.LayoutFullscreen);

        // システムウィンドウのデコレーションを無効化
        // WindowCompat.SetDecorFitsSystemWindows(Window, false);

        // レイアウトを画面全体に拡張
        Window.SetFlags(WindowManagerFlags.LayoutNoLimits, WindowManagerFlags.LayoutNoLimits);

        // ステータスバーを透過
        Window.SetStatusBarColor(new Android.Graphics.Color(ContextCompat.GetColor(this, Android.Resource.Color.Transparent)));

        // // display cutout領域にも表示
        // var attributes = Window.Attributes;
        // attributes.LayoutInDisplayCutoutMode = (int)WindowManagerLayoutParams.LayoutInDisplayCutoutModeShortEdges;
        // Window.Attributes = attributes;

    }
}
