using Xunit;
using FluentAssertions;
using Moq;
using MyApp.ViewModels;
using MyApp.Repositories;

public class BorrowBookViewModelTests
{
    [Fact]
    public void Borrow_ShouldReturnError_WhenQueryIsEmpty()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new BorrowBookViewModel(repo.Object);

        vm.Query = "";

        bool result = vm.Borrow();

        result.Should().BeFalse();
        vm.Message.Should().Be("Please enter a valid book ID or title.");
        vm.MessageColor.Should().Be("Error");

        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void Borrow_ShouldSucceed_WhenRepositoryReturnsSuccessMessage()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new BorrowBookViewModel(repo.Object);

        vm.Query = "123";

        repo.Setup(r => r.BorrowBookByQuery("123"))
            .Returns("Book borrowed successfully!");

        bool result = vm.Borrow();

        result.Should().BeTrue();
        vm.Message.Should().Be("Book borrowed successfully!");
        vm.MessageColor.Should().Be("Success");

        repo.Verify(r => r.BorrowBookByQuery("123"), Times.Once);
    }

    [Fact]
    public void Borrow_ShouldReturnError_WhenRepositoryReturnsErrorMessage()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new BorrowBookViewModel(repo.Object);

        vm.Query = "123";

        repo.Setup(r => r.BorrowBookByQuery("123"))
            .Returns("Book not found.");

        bool result = vm.Borrow();

        result.Should().BeFalse();
        vm.Message.Should().Be("Book not found.");
        vm.MessageColor.Should().Be("Error");

        repo.Verify(r => r.BorrowBookByQuery("123"), Times.Once);
    }

    [Fact]
    public void Borrow_ShouldReturnError_WhenBookIsAlreadyBorrowed()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new BorrowBookViewModel(repo.Object);

        vm.Query = "123";

        repo.Setup(r => r.BorrowBookByQuery("123"))
            .Returns("Book is already borrowed.");

        bool result = vm.Borrow();

        result.Should().BeFalse();
        vm.Message.Should().Be("Book is already borrowed.");
        vm.MessageColor.Should().Be("Error");

        repo.Verify(r => r.BorrowBookByQuery("123"), Times.Once);
    }

    [Fact]
    public void Cancel_ShouldClearQuery_AndMessage()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new BorrowBookViewModel(repo.Object);

        vm.Query = "123";
        repo.Setup(r => r.BorrowBookByQuery("123")).Returns("Book borrowed successfully!");
        var preResult = vm.Borrow();
        preResult.Should().BeTrue();

        vm.Cancel();

        vm.Query.Should().Be("");
        vm.Message.Should().Be("");
    }
}