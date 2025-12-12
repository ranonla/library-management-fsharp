using Avalonia.Controls;
using System.Collections.Generic;

namespace MyApp.Services;

public class NavigationService
{
    private readonly Stack<UserControl> _history = new();

    public UserControl? CurrentPage { get; private set; }

    public void Navigate(UserControl page)
    {
        if (CurrentPage != null)
            _history.Push(CurrentPage);

        CurrentPage = page;
    }

    public void GoBack(UserControl defaultPage)
    {
        CurrentPage = _history.Count > 0
            ? _history.Pop()
            : defaultPage;
    }
}