using System.Collections.Generic;
using MyApp.Repositories;
using static LibraryCore.BookModel;

namespace MyApp.ViewModels;

public class ReadAllBooksViewModel
{
    private readonly IBookRepository _repo;

    public List<Book> Books { get; private set; }

    public ReadAllBooksViewModel(IBookRepository repo)
    {
        _repo = repo;
        Books = _repo.GetAllBooks();
    }
}