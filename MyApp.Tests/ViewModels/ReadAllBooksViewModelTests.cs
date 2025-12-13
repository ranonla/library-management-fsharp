using Xunit;
using FluentAssertions;
using Moq;
using MyApp.ViewModels;
using MyApp.Repositories;
using static LibraryCore.BookModel;

public class ReadAllBooksViewModelTests
{
    private Book CreateBook(string title, string author)
    {
        return new Book(Guid.NewGuid(), title, author, BookStatus.Available);
    }

    [Fact]
    public void Constructor_ShouldLoadBooks_FromRepository()
    {
        var books = new List<Book>
        {
            CreateBook("Book A", "Author A"),
            CreateBook("Book B", "Author B")
        };

        var repo = new Mock<IBookRepository>();
        repo.Setup(r => r.GetAllBooks()).Returns(books);

        var vm = new ReadAllBooksViewModel(repo.Object);

        vm.Books.Should().NotBeNull();
        vm.Books.Should().HaveCount(2);
        vm.Books.Should().Contain(books[0]);
        vm.Books.Should().Contain(books[1]);

        repo.Verify(r => r.GetAllBooks(), Times.Once);
    }

    [Fact]
    public void Constructor_ShouldHandleEmptyList()
    {
        var repo = new Mock<IBookRepository>();
        repo.Setup(r => r.GetAllBooks()).Returns(new List<Book>());

        var vm = new ReadAllBooksViewModel(repo.Object);

        vm.Books.Should().NotBeNull();
        vm.Books.Should().BeEmpty();

        repo.Verify(r => r.GetAllBooks(), Times.Once);
    }
}