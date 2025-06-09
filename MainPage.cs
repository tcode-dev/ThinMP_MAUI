﻿namespace ThinMPm;

using CommunityToolkit.Maui.Markup;

using static CommunityToolkit.Maui.Markup.GridRowsColumns;

public class ViewModel
{
    public string RegistrationCode { get; set; }
}

class MainPage : ContentPage
{
    public MainPage()
    {
        Content = new Grid
        {
            RowDefinitions = Rows.Define(
                (Row.TextEntry, 36)),

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
                 .Bind(Entry.TextProperty, static (ViewModel vm) => vm.RegistrationCode, static (ViewModel vm, string text) => vm.RegistrationCode = text)
            }
        };
    }

    enum Row { TextEntry }
    enum Column { Description, Input }
}