using System;
using System.Collections.Generic;
using static LibraryCore.BookModel;
using static LibraryCore.BookManager;

namespace MyApp.Repositories;

public interface IBookRepository
{
    void UpdateBookTitle(Guid id, string newTitle);
    Book InsertBook(string title, string author);
    void UpdateAuthor(Guid id, string newAuthor);
    List<Book> SearchBooks(string query);
    string BorrowBookByQuery(string query);
    string ReturnBookByQuery(string query);
    string DeleteBookByQuery(string query);
    List<Book> GetAllBooks();

}