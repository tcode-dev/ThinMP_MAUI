using CommunityToolkit.Maui.Markup;

class SecondPage : ContentPage
{
    public SecondPage()
    {
        Content = new VerticalStackLayout
        {
            Children =
            {
                new Label()
                    .Text("Second Page")
                    .FontSize(24)
                    .CenterHorizontal(),

                new Button()
                    .Text("Back")
                    .CenterHorizontal()
                    .Invoke(b => b.Clicked += (s, e) => Navigation.PopAsync())
            }
        };
    }
}