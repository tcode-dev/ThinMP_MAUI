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
        BindingContext = new ViewModel(); // 追加

        Content = new Grid
        {
            RowDefinitions = Rows.Define(
                (Row.TextEntry, 36), (Row.Navigate, 60)),

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
                 .Bind(Entry.TextProperty, static (ViewModel vm) => vm.RegistrationCode, static (ViewModel vm, string text) => vm.RegistrationCode = text),

                // new Button()
                //     .Text("Go to Second Page")
                //     .Row(Row.Navigate).ColumnSpan(All<Column>())
                //     .CenterHorizontal()
                //     .Invoke(b => b.Clicked += async (s, e) =>
                //     {
                //         await Navigation.PushAsync(new SecondPage());
                //     }),

                new Button()
                    .Text("Go to Songs Page")
                    .Row(Row.Navigate).ColumnSpan(All<Column>())
                    .CenterHorizontal()
                    .Invoke(b => b.Clicked += async (s, e) =>
                    {
                        var page = Application.Current?.Handler?.MauiContext?.Services.GetRequiredService<SongsPage>();
                        await Navigation.PushAsync(page);
                        // var serviceProvider = Application.Current?.Handler?.MauiContext?.Services;
                        // if (serviceProvider is not null)
                        // {
                        //     var songsPage = serviceProvider.GetRequiredService<SongsPage>();
                        //     if (songsPage is not null)
                        //     {

                        //         await Navigation.PushAsync(songsPage);
                        //     }
                        // }
                    }),
            }
        };
    }

    enum Row { TextEntry, Navigate }
    enum Column { Description, Input }
}