using System;
using MyApp.Repositories;

namespace MyApp.ViewModels;

public class DeleteBookViewModel
{
    private readonly IBookRepository _repo;

    public string Query { get; set; } = "";
    public string Message { get; private set; } = "";
    public string MessageColor { get; private set; } = "";

    public DeleteBookViewModel(IBookRepository repo)
    {
        _repo = repo;
    }

    public bool Delete()
    {
        Message = "";

        if (string.IsNullOrWhiteSpace(Query))
        {
            Message = "Please enter a valid book ID or title.";
            MessageColor = "Error";
            return false;
        }

        var result = _repo.DeleteBookByQuery(Query);

        Message = result;

        if (result.Equals("Book deleted successfully!", StringComparison.OrdinalIgnoreCase))
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