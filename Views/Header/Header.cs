using CommunityToolkit.Maui.Markup;

namespace ThinMPm.Views.Header;

public class Header : ContentView
{
    public static readonly BindableProperty TitleProperty =
        BindableProperty.Create(
            nameof(Title),
            typeof(string),
            typeof(Header),
            default(string));

    public string Title
    {
        get => (string)GetValue(TitleProperty);
        set => SetValue(TitleProperty, value);
    }

    public Header()
    {
        HeightRequest = 50;
        BackgroundColor = Colors.White;

        Content = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(50),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(50)
            },
            Children =
            {
                new Button
                {
                    Text = "←",
                    FontSize = 18,
                    BackgroundColor = Colors.Transparent
                }.Column(0),

                new Label()
                    .Bind(Label.TextProperty, nameof(Title), source: this)
                    .Font(bold: true)
                    .Center()
                    .Column(1),
            }
        };
    }
}