namespace ThinMPm.Views.Buttons;

public class BackButton : Microsoft.Maui.Controls.Button
{
    public BackButton()
    {
        Text = "←";
        FontSize = 18;
        BackgroundColor = Colors.Transparent;
        WidthRequest = 50;
        HeightRequest = 50;

        Clicked += async (s, e) => await Shell.Current.GoToAsync("..");
    }
}
