namespace ThinMPm.Views.Header;

public class Header : ContentView
{
    public Header(string title)
    {
        HeightRequest = 50;

        Content = new HorizontalStackLayout
        {
            Children =
            {
                new Label
                {
                    Text = title
                }
            }
        };
    }
}