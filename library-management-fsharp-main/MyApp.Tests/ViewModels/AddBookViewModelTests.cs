using Xunit;
using FluentAssertions;
using Moq;
using MyApp.ViewModels;
using MyApp.Repositories;

public class AddBookViewModelTests
{
    [Fact]
    public void Add_ShouldReturnError_WhenFieldsAreEmpty()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new AddBookViewModel(repo.Object);

        vm.Title = "";
        vm.Author = "";

        bool result = vm.Add();

        result.Should().BeFalse();
        vm.Message.Should().Be("Please fill in all required fields.");
        vm.MessageColor.Should().Be("Error");

        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void Add_ShouldReturnError_WhenOnlyTitleIsProvided()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new AddBookViewModel(repo.Object);

        vm.Title = "Some Title";
        vm.Author = "";

        bool result = vm.Add();

        result.Should().BeFalse();
        vm.Message.Should().Be("Please fill in all required fields.");
        vm.MessageColor.Should().Be("Error");

        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void Add_ShouldReturnError_WhenOnlyAuthorIsProvided()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new AddBookViewModel(repo.Object);

        vm.Title = "";
        vm.Author = "Someone";

        bool result = vm.Add();

        result.Should().BeFalse();
        vm.Message.Should().Be("Please fill in all required fields.");
        vm.MessageColor.Should().Be("Error");

        repo.VerifyNoOtherCalls();
    }

    [Fact]
    public void Add_ShouldSucceed_WhenValidInputProvided()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new AddBookViewModel(repo.Object);

        vm.Title = "Test Title";
        vm.Author = "Author Name";

        bool result = vm.Add();

        result.Should().BeTrue();

        vm.Message.Should().Be("Book added successfully!");
        vm.MessageColor.Should().Be("Success");

        vm.Title.Should().Be("");
        vm.Author.Should().Be("");

        repo.Verify(r => r.InsertBook("Test Title", "Author Name"), Times.Once);
    }

    [Fact]
    public void Cancel_ShouldClearFields_AndMessage()
    {
        var repo = new Mock<IBookRepository>();
        var vm = new AddBookViewModel(repo.Object);

        vm.Title = "Something";
        vm.Author = "Someone";

        vm.Add();

        vm.Cancel();

        vm.Title.Should().Be("");
        vm.Author.Should().Be("");
        vm.Message.Should().Be("");
    }
}