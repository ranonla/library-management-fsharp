using System;
using System.Collections.Generic;
using LibraryCore;
using static LibraryCore.BookModel; 

namespace MyApp.Repositories
{
    public class BookRepository : IBookRepository
    {
        public void UpdateBookTitle(Guid id, string newTitle) =>
            BookManager.updateBookTitle(id, newTitle);

        public void UpdateAuthor(Guid id, string newAuthor) =>
            BookManager.updateAuthor(id, newAuthor);

        public List<Book> GetAllBooks() =>
        new List<Book>(BookManager.getAllBooks());

        public Book InsertBook(string title, string author) =>
            BookManager.insertBook(title, author);

        public string BorrowBookByQuery(string query)
        {
            var result = BookManager.borrowBookByQuery(query);
            var value = Microsoft.FSharp.Core.OptionModule.ToObj(result) as string;

            if (value == null)
                return "Book not found.";

            return value.Trim();
        }

        public string ReturnBookByQuery(string query)
        {
            var result = BookManager.returnBookByQuery(query);
            var value = Microsoft.FSharp.Core.OptionModule.ToObj(result) as string;

            return value?.Trim() ?? "Book not found.";
        }


        public string DeleteBookByQuery(string query)
        {
            var result = BookManager.deleteBookByQuery(query);
            var value = Microsoft.FSharp.Core.OptionModule.ToObj(result) as string;
            return value?.Trim() ?? "Book not found.";
        }

        public List<Book> SearchBooks(string query) =>
            new List<Book>(BookManager.searchBooks(query));


    }
}