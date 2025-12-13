using Xunit;
using FluentAssertions;
using Moq;
using MyApp.ViewModels;
using MyApp.Repositories;
using System;
using static LibraryCore.BookModel;

public class UpdateBookViewModelTests
{
    private Book CreateBook(string title, string author)
    {
        return new Book(Guid.NewGuid(), title, author, BookStatus.Available);
    }

    [Fact]
    public void Save_ShouldReturnError_WhenNothingChanged()
    {
        var book = CreateBook("Old Title", "Old Author");
        var repo = new Mock<IBookRepository>();
        var vm = new UpdateBookViewModel(book, repo.Object);

        bool result = vm.Save();

        result.Should().BeFalse();
        vm.Message.Should().Be("Please update your info.");
        vm.MessageColor.Should().Be("Error");

        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void Save_ShouldUpdate_WhenFieldsChanged()
    {
        var book = CreateBook("Old Title", "Old Author");
        var repo = new Mock<IBookRepository>();
        var vm = new UpdateBookViewModel(book, repo.Object);

        vm.Title = "New Title";
        vm.Author = "New Author";

        bool result = vm.Save();

        result.Should().BeTrue();
        vm.Message.Should().Be("Book updated successfully!");
        vm.MessageColor.Should().Be("Success");

        repo.Verify(r => r.UpdateBookTitle(book.Id, "New Title"), Times.Once);
        repo.Verify(r => r.UpdateAuthor(book.Id, "New Author"), Times.Once);
    }

    [Fact]
    public void Cancel_ShouldRestoreOriginalValues()
    {
        var book = CreateBook("Old Title", "Old Author");
        var repo = new Mock<IBookRepository>();
        var vm = new UpdateBookViewModel(book, repo.Object);

        vm.Title = "Something Else";
        vm.Author = "Another Thing";

        vm.Cancel();

        vm.Title.Should().Be("Old Title");
        vm.Author.Should().Be("Old Author");
        vm.Message.Should().Be("");
    }
}