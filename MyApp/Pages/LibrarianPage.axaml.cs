using Avalonia.Controls;
using Avalonia;
using MyApp.Pages;

namespace MyApp.Pages;

public partial class LibrarianPage : UserControl
{
    public LibrarianPage()
    {
        InitializeComponent();
        AddBookBtn.Click += (s, e) =>
        {
            var main = TopLevel.GetTopLevel(this) as MainWindow;
            main!.Navigate(new AddBookPage());
        };

        UpdateBookBtn.Click += (s, e) =>
        {
            var main = TopLevel.GetTopLevel(this) as MainWindow;
            main!.Navigate(new SearchBookPage());
        };

        DeleteBookBtn.Click += (s, e) =>
        {
            var main = TopLevel.GetTopLevel(this) as MainWindow;
            main!.Navigate(new DeleteBookPage());
        };

        BorrowBookBtn.Click += (s, e) =>
        {
            var main = TopLevel.GetTopLevel(this) as MainWindow;
            main!.Navigate(new BorrowBookPage());
        };

        ReturnBookBtn.Click += (s, e) =>
        {
            var main = TopLevel.GetTopLevel(this) as MainWindow;
            main!.Navigate(new ReturnBookPage());
        };

        ReadBooksBtn.Click += (s, e) =>
        {
            var main = TopLevel.GetTopLevel(this) as MainWindow;
            main!.Navigate(new ReadAllBooksPage());
        };

        BackButton.Click += (_, __) => {
        var main = (MainWindow)TopLevel.GetTopLevel(this);
        main.GoBack();
        };

    }
}