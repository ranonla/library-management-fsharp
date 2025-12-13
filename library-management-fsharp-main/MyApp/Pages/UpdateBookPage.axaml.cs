using Avalonia.Controls;
using static LibraryCore.BookModel;
using MyApp.Repositories;

namespace MyApp.Pages;

public partial class UpdateBookPage : UserControl
{
    private readonly UpdateBookViewModel _vm;

    public UpdateBookPage(Book book)
    {
        InitializeComponent();

        _vm = new UpdateBookViewModel(book, new BookRepository());
        DataContext = _vm;

        RefreshUI();

        SaveButton.Click += OnSave;
        CancelButton.Click += OnCancel;
        BackButton.Click += OnBack;
    }

    private void OnBack(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        var main = (MainWindow)TopLevel.GetTopLevel(this);
        main.GoBack();
    }

    private void OnCancel(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _vm.Cancel();
        RefreshUI();
    }

    private void OnSave(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _vm.Title = TitleBox.Text ?? "";
        _vm.Author = AuthorBox.Text ?? "";

        var success = _vm.Save();

        RefreshUI(success);
    }

    private void RefreshUI(bool? success = null)
    {
        TitleBox.Text = _vm.Title;
        AuthorBox.Text = _vm.Author;

        MessageText.Text = _vm.Message;

        if (success != null)
        {
            MessageText.Foreground = success == true
                ? Avalonia.Media.Brushes.LightGreen
                : Avalonia.Media.Brushes.OrangeRed;
        }
        else
        {
            MessageText.Foreground = Avalonia.Media.Brushes.White;
        }
    }
}
