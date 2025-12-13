using Xunit;
using FluentAssertions;
using Moq;
using MyApp.ViewModels;
using MyApp.Repositories;
using System.Collections.Generic;
using static LibraryCore.BookModel;
using System;

public class SearchBookViewModelTests
{
    private Book CreateBook(string title, string author)
    {
        return new Book(Guid.NewGuid(), title, author, BookStatus.Available);
    }

    [Fact]
    public void Search_ShouldReturnError_WhenQueryIsEmpty()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new SearchBookViewModel(repo.Object);

        vm.Query = "";

        bool result = vm.Search();

        result.Should().BeFalse();
        vm.Message.Should().Be("Please enter a valid search value.");
        vm.MessageColor.Should().Be("Error");

        vm.Results.Should().BeEmpty();

        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void Search_ShouldReturnError_WhenNoBooksFound()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new SearchBookViewModel(repo.Object);

        vm.Query = "Unknown";

        repo.Setup(r => r.SearchBooks("Unknown"))
            .Returns(new List<Book>());

        bool result = vm.Search();

        result.Should().BeFalse();
        vm.Message.Should().Be("Book not found.");
        vm.MessageColor.Should().Be("Error");
        vm.Results.Should().BeEmpty();

        repo.Verify(r => r.SearchBooks("Unknown"), Times.Once);
    }

    [Fact]
    public void Search_ShouldSucceed_WhenBooksAreFound()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new SearchBookViewModel(repo.Object);

        vm.Query = "Harry";

        var results = new List<Book>
        {
            CreateBook("Harry Potter", "J.K. Rowling"),
            CreateBook("Harry Potter: Chamber of Secrets", "J.K. Rowling")
        };

        repo.Setup(r => r.SearchBooks("Harry"))
            .Returns(results);

        bool result = vm.Search();

        result.Should().BeTrue();
        vm.Message.Should().Be("");
        vm.Results.Should().HaveCount(2);
        vm.Results.Should().BeEquivalentTo(results);

        repo.Verify(r => r.SearchBooks("Harry"), Times.Once);
    }

    [Fact]
    public void Search_ShouldReplaceOldResultsOnNewSearch()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new SearchBookViewModel(repo.Object);

        vm.Results.Add(CreateBook("OldBook", "OldAuthor"));
        vm.Query = "New";

        var newResults = new List<Book>
        {
            CreateBook("NewBook1", "Author1"),
            CreateBook("NewBook2", "Author2"),
        };

        repo.Setup(r => r.SearchBooks("New"))
            .Returns(newResults);

        bool result = vm.Search();

        result.Should().BeTrue();
        vm.Results.Should().HaveCount(2);
        vm.Results.Should().BeEquivalentTo(newResults);
    }
}