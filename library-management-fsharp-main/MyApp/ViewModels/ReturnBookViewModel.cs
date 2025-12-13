using System;
using MyApp.Repositories;

namespace MyApp.ViewModels;

public class ReturnBookViewModel
{
    private readonly IBookRepository _repo;

    public string Query { get; set; } = "";
    public string Message { get; private set; } = "";
    public string MessageColor { get; private set; } = "";

    public ReturnBookViewModel(IBookRepository repo)
    {
        _repo = repo;
    }

    public bool ReturnBook()
    {
        Message = "";

        if (string.IsNullOrWhiteSpace(Query))
        {
            Message = "Please enter a valid book ID or title.";
            MessageColor = "Error";
            return false;
        }

        var result = _repo.ReturnBookByQuery(Query);
        Message = result;

        if (result.Equals("Book returned successfully!", StringComparison.OrdinalIgnoreCase))
        {
            MessageColor = "Success";
            return true;
        }

        MessageColor = "Error";
        return false;
    }

    public void Cancel()
    {
        Query = "";
        Message = "";
    }
}