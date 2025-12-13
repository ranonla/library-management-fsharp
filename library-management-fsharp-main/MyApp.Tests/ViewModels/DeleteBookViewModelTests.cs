using Xunit;
using FluentAssertions;
using Moq;
using MyApp.ViewModels;
using MyApp.Repositories;

public class DeleteBookViewModelTests
{
    [Fact]
    public void Delete_ShouldReturnError_WhenQueryIsEmpty()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new DeleteBookViewModel(repo.Object);

        vm.Query = "";

        bool result = vm.Delete();

        result.Should().BeFalse();
        vm.Message.Should().Be("Please enter a valid book ID or title.");
        vm.MessageColor.Should().Be("Error");

        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void Delete_ShouldSucceed_WhenRepositoryReturnsSuccessMessage()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new DeleteBookViewModel(repo.Object);

        vm.Query = "123";

        repo.Setup(r => r.DeleteBookByQuery("123"))
            .Returns("Book deleted successfully!");

        bool result = vm.Delete();

        result.Should().BeTrue();
        vm.Message.Should().Be("Book deleted successfully!");
        vm.MessageColor.Should().Be("Success");

        repo.Verify(r => r.DeleteBookByQuery("123"), Times.Once);
    }

    [Fact]
    public void Delete_ShouldReturnError_WhenBookNotFound()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new DeleteBookViewModel(repo.Object);

        vm.Query = "unknown";

        repo.Setup(r => r.DeleteBookByQuery("unknown"))
            .Returns("Book not found.");

        bool result = vm.Delete();

        result.Should().BeFalse();
        vm.Message.Should().Be("Book not found.");
        vm.MessageColor.Should().Be("Error");

        repo.Verify(r => r.DeleteBookByQuery("unknown"), Times.Once);
    }

    [Fact]
    public void Delete_ShouldReturnError_WhenRepositoryReturnsOtherError()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new DeleteBookViewModel(repo.Object);

        vm.Query = "500";

        repo.Setup(r => r.DeleteBookByQuery("500"))
            .Returns("Could not delete book.");

        bool result = vm.Delete();

        result.Should().BeFalse();
        vm.Message.Should().Be("Could not delete book.");
        vm.MessageColor.Should().Be("Error");

        repo.Verify(r => r.DeleteBookByQuery("500"), Times.Once);
    }

    [Fact]
    public void Cancel_ShouldClearQuery_AndMessage()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new DeleteBookViewModel(repo.Object);

        vm.Query = "123";

        repo.Setup(r => r.DeleteBookByQuery("123")).Returns("Book deleted successfully!");
        vm.Delete();

        vm.Cancel();

        vm.Query.Should().Be("");
        vm.Message.Should().Be("");
    }
}