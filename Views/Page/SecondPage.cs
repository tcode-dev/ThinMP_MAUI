using System.Collections.ObjectModel;
using CommunityToolkit.Maui.Markup;

class SecondPage : ContentPage
{
    public SecondPage()
    {
        // Content = new VerticalStackLayout
        // {
        //     Children =
        //     {
        //         new Label()
        //             .Text("Second Page")
        //             .FontSize(24)
        //             .CenterHorizontal(),

        //         new Button()
        //             .Text("Back")
        //             .CenterHorizontal()
        //             .Invoke(b => b.Clicked += (s, e) => Navigation.PopAsync()),

        //     }
        // };
        var items = new ObservableCollection<string>
        {
            "aa", "bb", "cc"
        };

        Content = new CollectionView
        {
            ItemsSource = items,
            ItemTemplate = new DataTemplate(() =>
                new Label()
                    .Bind(Label.TextProperty, ".")
                    .FontSize(20)
                    .Padding(10)
            )
        }
        .Margin(20);

    }
}