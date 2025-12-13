using System.Collections.Generic;
using MyApp.Repositories;
using static LibraryCore.BookModel;

namespace MyApp.ViewModels;

public class SearchBookViewModel
{
    private readonly IBookRepository _repo;
    public string Query { get; set; } = "";
    public string Message { get; private set; } = "";
    public string MessageColor { get; private set; } = "";
    public List<Book> Results { get; private set; } = new();
    public SearchBookViewModel(IBookRepository repo)
    {
        _repo = repo;
    }

    public bool Search()
    {
        Message = "";
        Results.Clear();

        if (string.IsNullOrWhiteSpace(Query))
        {
            Message = "Please enter a valid search value.";
            MessageColor = "Error";
            return false;
        }

        var found = _repo.SearchBooks(Query);

        if (found.Count == 0)
        {
            Message = "Book not found.";
            MessageColor = "Error";
            return false;
        }

        Results = found;
        Message = "";
        return true;
    }
}