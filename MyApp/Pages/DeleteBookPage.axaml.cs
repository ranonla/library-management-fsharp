using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MyApp.Repositories;
using MyApp.ViewModels;

namespace MyApp.Pages;

public partial class DeleteBookPage : UserControl
{
    private readonly DeleteBookViewModel _vm;

    public DeleteBookPage()
    {
        InitializeComponent();

        _vm = new DeleteBookViewModel(new BookRepository());
        DataContext = _vm;

        DeleteButton.Click += OnDeleteBook;
        CancelButton.Click += OnCancel;

        BackButton.Click += (_, __) =>
        {
            var main = (MainWindow)TopLevel.GetTopLevel(this);
            main.GoBack();
        };
    }

    private void OnDeleteBook(object? sender, RoutedEventArgs e)
    {
        _vm.Query = TextBox.Text ?? "";

        bool success = _vm.Delete();

        MessageText.Text = _vm.Message;
        MessageText.Foreground = success
            ? Brushes.LightGreen
            : Brushes.OrangeRed;
    }

    private void OnCancel(object? sender, RoutedEventArgs e)
    {
        _vm.Cancel();

        TextBox.Text = "";
        MessageText.Text = "";
    }
}