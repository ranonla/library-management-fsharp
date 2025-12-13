using Xunit;
using FluentAssertions;
using Avalonia.Controls;
using MyApp.Services;

public class NavigationServiceTests
{
    private class DummyPage : UserControl {}

    [Fact]
    public void Navigate_ShouldSetCurrentPage()
    {
        var service = new NavigationService();
        var page = new DummyPage();

        service.Navigate(page);

        service.CurrentPage.Should().Be(page);
    }

    [Fact]
    public void Navigate_ShouldPushPreviousPageToHistory()
    {
        var service = new NavigationService();
        var first = new DummyPage();
        var second = new DummyPage();

        service.Navigate(first);
        service.Navigate(second);

        service.GoBack(new DummyPage());
        service.CurrentPage.Should().Be(first);
    }

    [Fact]
    public void GoBack_ShouldReturnDefault_WhenNoHistory()
    {
        var service = new NavigationService();
        var defaultPage = new DummyPage();

        service.GoBack(defaultPage);
        service.CurrentPage.Should().Be(defaultPage);
    }
}
