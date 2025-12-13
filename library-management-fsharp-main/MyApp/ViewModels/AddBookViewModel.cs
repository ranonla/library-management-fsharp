using MyApp.Repositories;

namespace MyApp.ViewModels;

public class AddBookViewModel
{
    private readonly IBookRepository _repo;

    public string Title { get; set; } = "";
    public string Author { get; set; } = "";
    public string Message { get; private set; } = "";
    public string MessageColor { get; private set; } = "";

    public AddBookViewModel(IBookRepository repo)
    {
        _repo = repo;
    }

    public bool Add()
    {
        if (string.IsNullOrWhiteSpace(Title) || string.IsNullOrWhiteSpace(Author))
        {
            Message = "Please fill in all required fields.";
            MessageColor = "Error";
            return false;
        }

        _repo.InsertBook(Title, Author);

        Message = "Book added successfully!";
        MessageColor = "Success";

        Title = "";
        Author = "";

        return true;
    }

    public void Cancel()
    {
        Title = "";
        Author = "";
        Message = "";
    }
}