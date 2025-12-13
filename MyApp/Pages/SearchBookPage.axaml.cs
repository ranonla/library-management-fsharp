using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Media;
using MyApp.ViewModels;
using MyApp.Repositories;
using static LibraryCore.BookModel;

namespace MyApp.Pages;

public partial class SearchBookPage : UserControl
{
    private readonly SearchBookViewModel _vm;

    public SearchBookPage()
    {
        InitializeComponent();

        _vm = new SearchBookViewModel(new BookRepository());
        DataContext = _vm;

        SearchButton.Click += OnSearchClicked;
        BackButton.Click += (_, __) =>
        {
            var main = (MainWindow)TopLevel.GetTopLevel(this);
            main.GoBack();
        };
    }

    private void OnSearchClicked(object? sender, RoutedEventArgs e)
    {
        _vm.Query = SearchBox.Text ?? "";

        bool success = _vm.Search();
        //clear old ones
        ResultsPanel.ItemsSource = null;

        if (!success)
        {
            ShowMessage(_vm.Message, Brushes.OrangeRed);
            ResultsPanel.IsVisible = false;
            return;
        }

        // Show results
        ResultsPanel.ItemsSource = _vm.Results;
        ResultsPanel.IsVisible = true;
        MessageText.IsVisible = false;
    }

    private void OnResultEditClicked(object? sender, RoutedEventArgs e)
    {
        if (sender is Button btn && btn.Tag is Book book)
        {
            var main = (MainWindow)TopLevel.GetTopLevel(this);
            main.MainContent.Content = new UpdateBookPage(book);
        }
    }

    private void ShowMessage(string message, IBrush color)
    {
        MessageText.Text = message;
        MessageText.Foreground = color;
        MessageText.IsVisible = true;
    }
}