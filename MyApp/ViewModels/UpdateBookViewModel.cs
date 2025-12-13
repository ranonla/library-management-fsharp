using MyApp.Repositories;
using static LibraryCore.BookModel;

public class UpdateBookViewModel
{
    private readonly Book _originalBook;
    private readonly IBookRepository _repo;
    public string Title { get; set; }
    public string Author { get; set; }
    public string Message { get; private set; }
    public string MessageColor { get; private set; }
    public UpdateBookViewModel(Book book, IBookRepository repo)
    {
        _originalBook = book;
        _repo = repo;

        Title = book.Title;
        Author = book.Author;
    }

    public bool Save()
    {
        if (Title == _originalBook.Title &&
            Author == _originalBook.Author)
        {
            Message = "Please update your info.";
            MessageColor = "Error";
            return false;
        }

        _repo.UpdateBookTitle(_originalBook.Id, Title);
        _repo.UpdateAuthor(_originalBook.Id, Author);

        Message = "Book updated successfully!";
        MessageColor = "Success";
        return true;
    }

    public void Cancel()
    {
        Title = _originalBook.Title;
        Author = _originalBook.Author;
        Message = "";
    }
}
