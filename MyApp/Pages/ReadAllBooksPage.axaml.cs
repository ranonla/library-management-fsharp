using Avalonia.Controls;
using MyApp.Repositories;
using MyApp.ViewModels;

namespace MyApp.Pages;

public partial class ReadAllBooksPage : UserControl
{
    private readonly ReadAllBooksViewModel _vm;

    public ReadAllBooksPage()
    {
        InitializeComponent();

        _vm = new ReadAllBooksViewModel(new BookRepository());
        DataContext = _vm;

        BackButton.Click += (_, __) =>
        {
            var main = (MainWindow)TopLevel.GetTopLevel(this);
            main.GoBack();
        };
    }
}