using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MyApp.Repositories;
using MyApp.ViewModels;

namespace MyApp.Pages;

public partial class ReturnBookPage : UserControl
{
    private readonly ReturnBookViewModel _vm;

    public ReturnBookPage()
    {
        InitializeComponent();

        _vm = new ReturnBookViewModel(new BookRepository());
        DataContext = _vm;

        ReturnButton.Click += OnReturnBook;
        CancelButton.Click += OnCancel;

        BackButton.Click += (_, __) =>
        {
            var main = (MainWindow)TopLevel.GetTopLevel(this);
            main.GoBack();
        };
    }

    private void OnReturnBook(object? sender, RoutedEventArgs e)
    {
        _vm.Query = TextBox.Text ?? "";

        bool success = _vm.ReturnBook();

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