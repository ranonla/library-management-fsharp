using MyApp.Repositories;
namespace MyApp.ViewModels;
using System;

public class BorrowBookViewModel
{
    private readonly IBookRepository _repo;

    public string Query { get; set; } = "";
    public string Message { get; private set; } = "";
    public string MessageColor { get; private set; } = "";

    public BorrowBookViewModel(IBookRepository repo)
    {
        _repo = repo;
    }

    public bool Borrow()
    {
        Message = "";

        if (string.IsNullOrWhiteSpace(Query))
        {
            Message = "Please enter a valid book ID or title.";
            MessageColor = "Error";
            return false;
        }

        var result = _repo.BorrowBookByQuery(Query);

        Message = result;

        if (result.Equals("Book borrowed successfully!", StringComparison.OrdinalIgnoreCase))
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