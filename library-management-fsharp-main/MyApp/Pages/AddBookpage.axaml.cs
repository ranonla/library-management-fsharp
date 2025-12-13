using Avalonia.Controls;
using MyApp.Repositories;
using MyApp.ViewModels;

namespace MyApp.Pages;

public partial class AddBookPage : UserControl
{
    private readonly AddBookViewModel _vm;

    public AddBookPage()
    {
        InitializeComponent();

        _vm = new AddBookViewModel(new BookRepository());
        DataContext = _vm;

        AddButton.Click += OnAddBook;
        CancelButton.Click += OnCancel;

        BackButton.Click += (_, __) =>
        {
            var main = (MainWindow)TopLevel.GetTopLevel(this);
            main.GoBack();
        };
    }

    private void OnAddBook(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _vm.Title = TitleBox.Text ?? "";
        _vm.Author = AuthorBox.Text ?? "";

        bool success = _vm.Add();

        MessageText.Text = _vm.Message;
        MessageText.Foreground = success
            ? Avalonia.Media.Brushes.LightGreen
            : Avalonia.Media.Brushes.OrangeRed;

        // sync UI with ViewModel
        TitleBox.Text = _vm.Title;
        AuthorBox.Text = _vm.Author;
    }

    private void OnCancel(object? sender, Avalonia.Interactivity.RoutedEventArgs e)
    {
        _vm.Cancel();

        TitleBox.Text = "";
        AuthorBox.Text = "";
        MessageText.Text = "";
    }
}