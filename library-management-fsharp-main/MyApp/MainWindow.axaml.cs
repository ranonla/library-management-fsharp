using Avalonia.Controls;
using System.Collections.Generic;
using MyApp.Services;

namespace MyApp;

public partial class MainWindow : Window
{
    private readonly NavigationService _navigation = new();

    public MainWindow()
    {
        InitializeComponent();

        // Load initial page
        Navigate(new Pages.HomePage());
    }

    public void Navigate(UserControl page)
    {
        _navigation.Navigate(page);
        MainContent.Content = _navigation.CurrentPage;
    }

    public void GoBack()
    {
        _navigation.GoBack(new Pages.HomePage());
        MainContent.Content = _navigation.CurrentPage;
    }
}