using CommunityToolkit.Maui.Markup;
using static CommunityToolkit.Maui.Markup.GridRowsColumns;

namespace ThinMPm.Views.Page;

public class ViewModel
{
    public string RegistrationCode { get; set; }
}

class MainPage : ContentPage
{
    public MainPage()
    {
        NavigationPage.SetHasNavigationBar(this, false);

        BindingContext = new ViewModel();

        Content = new Grid
        {
            RowDefinitions = Rows.Define(
                (Row.TextEntry, 36), (Row.Artists, 60), (Row.Albums, 60), (Row.Songs, 60)),

            ColumnDefinitions = Columns.Define(
                (Column.Description, Star),
                (Column.Input, Stars(2))),

            Children =
            {
                new Label()
                    .Text("Code:")
                    .Row(Row.TextEntry).Column(Column.Description),

                new Entry
                {
                    Keyboard = Keyboard.Numeric,
                }.Row(Row.TextEntry).Column(Column.Input)
                 .BackgroundColor(Colors.AliceBlue)
                 .FontSize(15)
                 .Placeholder("Enter number")
                 .TextColor(Colors.Black)
                 .Height(44)
                 .Margin(5, 5)
                 .Bind(Entry.TextProperty, static (ViewModel vm) => vm.RegistrationCode, static (vm, text) => vm.RegistrationCode = text),
                new Button()
                    .Text("Go to Artists Page")
                    .Row(Row.Artists).ColumnSpan(All<Column>())
                    .CenterHorizontal()
                    .Invoke(b => b.Clicked += async (s, e) =>
                    {
                        var page = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<ArtistsPage>();

                        await Navigation.PushAsync(page);
                    }),
                new Button()
                    .Text("Go to Albums Page")
                    .Row(Row.Albums).ColumnSpan(All<Column>())
                    .CenterHorizontal()
                    .Invoke(b => b.Clicked += async (s, e) =>
                    {
                        var page = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<AlbumsPage>();

                        await Navigation.PushAsync(page);
                    }),
                new Button()
                    .Text("Go to Songs Page")
                    .Row(Row.Songs).ColumnSpan(All<Column>())
                    .CenterHorizontal()
                    .Invoke(b => b.Clicked += async (s, e) =>
                    {
                        var page = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<SongsPage>();

                        await Navigation.PushAsync(page);
                    }),


            }
        };
    }

    enum Row { TextEntry, Artists, Albums, Songs }
    enum Column { Description, Input }
}