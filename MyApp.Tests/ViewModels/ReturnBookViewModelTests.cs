using Xunit;
using FluentAssertions;
using Moq;
using MyApp.ViewModels;
using MyApp.Repositories;

public class ReturnBookViewModelTests
{
    [Fact]
    public void ReturnBook_ShouldReturnError_WhenQueryIsEmpty()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new ReturnBookViewModel(repo.Object);

        vm.Query = "";

        bool result = vm.ReturnBook();

        result.Should().BeFalse();
        vm.Message.Should().Be("Please enter a valid book ID or title.");
        vm.MessageColor.Should().Be("Error");

        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void ReturnBook_ShouldSucceed_WhenRepositoryReturnsSuccessMessage()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new ReturnBookViewModel(repo.Object);

        vm.Query = "123";

        repo.Setup(r => r.ReturnBookByQuery("123"))
            .Returns("Book returned successfully!");

        bool result = vm.ReturnBook();

        result.Should().BeTrue();
        vm.Message.Should().Be("Book returned successfully!");
        vm.MessageColor.Should().Be("Success");

        repo.Verify(r => r.ReturnBookByQuery("123"), Times.Once);
    }

    [Fact]
    public void ReturnBook_ShouldReturnError_WhenBookNotFound()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new ReturnBookViewModel(repo.Object);

        vm.Query = "999";

        repo.Setup(r => r.ReturnBookByQuery("999"))
            .Returns("Book not found.");

        bool result = vm.ReturnBook();

        result.Should().BeFalse();
        vm.Message.Should().Be("Book not found.");
        vm.MessageColor.Should().Be("Error");

        repo.Verify(r => r.ReturnBookByQuery("999"), Times.Once);
    }

    [Fact]
    public void ReturnBook_ShouldReturnError_WhenBookIsAlreadyAvailable()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new ReturnBookViewModel(repo.Object);

        vm.Query = "123";

        repo.Setup(r => r.ReturnBookByQuery("123"))
            .Returns("Book is already available.");

        bool result = vm.ReturnBook();

        result.Should().BeFalse();
        vm.Message.Should().Be("Book is already available.");
        vm.MessageColor.Should().Be("Error");

        repo.Verify(r => r.ReturnBookByQuery("123"), Times.Once);
    }

    [Fact]
    public void ReturnBook_ShouldReturnError_ForAnyOtherErrorMessage()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new ReturnBookViewModel(repo.Object);

        vm.Query = "123";

        repo.Setup(r => r.ReturnBookByQuery("123"))
            .Returns("Unexpected error.");

        bool result = vm.ReturnBook();

        result.Should().BeFalse();
        vm.Message.Should().Be("Unexpected error.");
        vm.MessageColor.Should().Be("Error");

        repo.Verify(r => r.ReturnBookByQuery("123"), Times.Once);
    }

    [Fact]
    public void Cancel_ShouldClearQuery_AndMessage()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new ReturnBookViewModel(repo.Object);

        vm.Query = "123";
        repo.Setup(r => r.ReturnBookByQuery("123"))
        .Returns("Book returned successfully!");

        vm.ReturnBook();

        vm.Cancel();

        vm.Query.Should().Be("");
        vm.Message.Should().Be("");
    }
}