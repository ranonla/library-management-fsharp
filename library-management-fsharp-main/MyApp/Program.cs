using Avalonia;
using System;
using LibraryCore;

namespace MyApp;

class Program
{
    [STAThread]
    public static void Main(string[] args)
    {
        // ⭐ Create database table BEFORE UI loads
        LibraryCore.BookManager.createTable();

        BuildAvaloniaApp()
            .StartWithClassicDesktopLifetime(args);
    }

    public static AppBuilder BuildAvaloniaApp()
        => AppBuilder.Configure<App>()
            .UsePlatformDetect()
            .WithInterFont()
            .LogToTrace();
}