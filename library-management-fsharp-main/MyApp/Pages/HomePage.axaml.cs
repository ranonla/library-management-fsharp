using Avalonia.Controls;
using Avalonia;

namespace MyApp.Pages;

public partial class HomePage : UserControl
{
    public HomePage()
    {
        InitializeComponent();
        
        var librarianBtn = this.FindControl<Button>("LibrarianButton");
        var visitorBtn   = this.FindControl<Button>("VisitorButton");

        librarianBtn.Click += (s, e) =>
        {
            var main = TopLevel.GetTopLevel(this) as MainWindow;
            main!.Navigate(new LibrarianPage());
        };

        visitorBtn.Click += (s, e) =>
        {
            var main = TopLevel.GetTopLevel(this) as MainWindow;
            main!.Navigate(new VisitorPage());
        };
    }
}