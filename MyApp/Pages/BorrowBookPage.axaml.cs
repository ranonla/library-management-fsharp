using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MyApp.ViewModels;
using MyApp.Repositories;

namespace MyApp.Pages;

public partial class BorrowBookPage : UserControl
{
    private readonly BorrowBookViewModel _vm;

    public BorrowBookPage()
    {
        InitializeComponent();

        _vm = new BorrowBookViewModel(new BookRepository());
        DataContext = _vm;

        BorrowButton.Click += OnBorrowBook;
        CancelButton.Click += OnCancel;

        BackButton.Click += (_, __) =>
        {
            var main = (MainWindow)TopLevel.GetTopLevel(this);
            main.GoBack();
        };
    }

    private void OnBorrowBook(object? sender, RoutedEventArgs e)
    {
        _vm.Query = TextBox.Text ?? "";

        var success = _vm.Borrow();

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