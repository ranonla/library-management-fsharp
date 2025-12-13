using Avalonia.Controls;
using Avalonia;

namespace MyApp.Pages;

public partial class VisitorPage : UserControl
{
    public VisitorPage()
    {
        InitializeComponent();

        ReadBooksBtn.Click += (_, __) => NavigateTo(new ReadAllBooksPage());
        BorrowBookBtn.Click += (_, __) => NavigateTo(new BorrowBookPage());
        ReturnBookBtn.Click += (_, __) => NavigateTo(new ReturnBookPage());

        BackBtn.Click += (_, __) =>
        {
            var main = (MainWindow)TopLevel.GetTopLevel(this);
            main.GoBack();
        };
    }

    private void NavigateTo(UserControl page)
    {
        var main = TopLevel.GetTopLevel(this) as MainWindow;
        main!.Navigate(page);
    }
}