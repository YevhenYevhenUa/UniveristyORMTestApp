using Microsoft.AspNetCore.Mvc;
using Moq;
using Task9.University.Services.Interfaces;
using Task9.University.Domain.Core;
using Task9.University.Infrastructure.Presentations;
using Task9University.Controllers;
using Task9.University.Infrastructure.Services;

namespace Task9.University.Tests.ControllersTests;

public class GroupsComtrollerTests
{
    public readonly GroupsController _sut;
    public readonly Mock<IGroupService> _groupServiceMock;
    private readonly CancellationToken _cancellationToken;
    public GroupsComtrollerTests()
    {
        _groupServiceMock = new Mock<IGroupService>();
        _sut = new GroupsController(_groupServiceMock.Object);
        _cancellationToken = new CancellationToken();
    }

    [Fact]
    public async Task GroupController_Index_ShouldReturnListOfGroups()
    {
        //Arrange
        int courseId = 1;
        var groupList = GetGroups();
        _groupServiceMock.Setup(x => x.Index(courseId, _cancellationToken)).ReturnsAsync(groupList);

        //Act
        var result = await _sut.Index(courseId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Group>>(viewResult.ViewData.Model);
        Assert.Equal(3, model.Count());
    }

    [Fact]
    public async Task GroupController_Index_ShouldReturnErrorViewIfNull()
    {
        //Arrange
        int courseId = 5;
        string expectedViewName = "Error";
        _groupServiceMock.Setup(x => x.Index(courseId, _cancellationToken)).ReturnsAsync(() => null);

        //Act
        var result = await _sut.Index(courseId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(viewResult.ViewName, expectedViewName);
    }


    [Fact]
    public async Task GroupController_Edit_ShouldReturnEditGroupViewModel()
    {
        //Act
        int groupId = 1;
        var group = GetGroups().FirstOrDefault(g => g.GroupId == groupId);
        var groupVM = new EditGroupViewModel
        {
            Id = group.GroupId,
            CourseId = (int)group.CourseId,
            Name = group.Name
        };
        _groupServiceMock.Setup(x => x.EditGroupView(groupId, _cancellationToken)).ReturnsAsync(groupVM);

        //Act
        var result = await _sut.Edit(groupId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<EditGroupViewModel>(viewResult.ViewData.Model);
        Assert.Equal(group.Name, model.Name);
    }

    [Fact]
    public async Task GroupController_Edit_ShouldReturnRedirectToRoute()
    {
        //Act
        int groupId = 1;
        string newName = "Test1";
        string expectedAction = "Index";
        var group = GetGroups().FirstOrDefault(g => g.GroupId == groupId);
        var groupVM = new EditGroupViewModel
        {
            Name = newName,
            CourseId = (int)group.CourseId,
            Id = group.GroupId
        };
        _groupServiceMock.Setup(x => x.GroupEdit(groupVM, _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.Edit(groupVM, _cancellationToken) as RedirectToRouteResult;

        //Assert
        Assert.Equal(expectedAction, result.RouteValues["action"]);

    }

    [Fact]
    public async Task GroupController_Edit_ShouldReturnErrorViewIfNull()
    {
        //Arrange
        int groupId = 5;
        string expectedViewName = "Error";
        _groupServiceMock.Setup(x => x.EditGroupView(groupId, _cancellationToken)).ReturnsAsync(() => null);

        //Act
        var result = await _sut.Edit(groupId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(viewResult.ViewName, expectedViewName);
    }

    [Fact]
    public async Task GroupController_Delete_ReturnGroup()
    {
        //Arrange
        int groupId = 1;
        var group = GetGroups().FirstOrDefault(x => x.GroupId == groupId);
        var groupVM = new DeleteGroupViewModel
        {
            CourseId= (int)group.CourseId,
            Id = group.GroupId,
            Name = group.Name
        };
        _groupServiceMock.Setup(x => x.DeleteGroupView(groupId, _cancellationToken)).ReturnsAsync(groupVM);

        //Act
        var result = await _sut.Delete(groupId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<DeleteGroupViewModel>(viewResult.ViewData.Model);
        Assert.Equal(group.Name, model.Name);
    }

    [Fact]
    public async Task GroupController_Delete_ShouldReturnRedirectToRoute()
    {
        //Arrange
        int groupId = 1;
        string expectAction = "Index";
        var group = GetGroups().FirstOrDefault(x => x.GroupId == groupId);
        var groupVM = new DeleteGroupViewModel
        {
            CourseId = (int)group.CourseId,
            Id = group.GroupId,
            Name = group.Name
        };
        _groupServiceMock.Setup(x => x.GroupDelete(groupVM, _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.Delete(groupVM, _cancellationToken) as RedirectToRouteResult;

        //Assert
        Assert.Equal(expectAction, result.RouteValues["Action"]);
    }

    [Fact]
    public async Task GroupController_Delete_ShouldReturnErrorViewIfNull()
    {
        //Arrange
        int groupId = 5;
        string expectedViewName = "Error";
        _groupServiceMock.Setup(x => x.DeleteGroupView(groupId, _cancellationToken)).ReturnsAsync(() => null);

        //Act
        var result = await _sut.Delete(groupId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(viewResult.ViewName, expectedViewName);
    }

    [Fact]
    public async Task GroupController_Create_ShouldReturnCreateGroupViewModel()
    {
        //Arrange
        int courseId = 1;
        var groupVM = new CreateGroupViewModel
        {
            Name = "testName",
            CourseId = courseId,
        };
        _groupServiceMock.Setup(x => x.CreateGroupView(courseId, _cancellationToken)).ReturnsAsync(groupVM);

        //Act
        var result = await _sut.Create(courseId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<CreateGroupViewModel>(viewResult.ViewData.Model);
        Assert.Equal(courseId, model.CourseId);
        Assert.Equal("testName", model.Name);

    }

    [Fact]
    public async Task GroupController_Create_ShouldReturnRedirectToRoute()
    {
        //Arrange
        string expectAction = "Index";
        var testData = new CreateGroupViewModel { Name = "Test", CourseId = 1 };
        _groupServiceMock.Setup(x => x.CreateGroup(testData, _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.Create(testData, _cancellationToken) as RedirectToRouteResult;

        //Assert
        Assert.Equal(expectAction, result.RouteValues["Action"]);
    }

    [Fact]
    public async Task GroupController_Create_ShouldReturnErrorViewIfNull()
    {
        //Arrange
        int groupId = 5;
        string expectedViewName = "Error";
        _groupServiceMock.Setup(x => x.CreateGroupView(groupId, _cancellationToken)).ReturnsAsync(() => null);

        //Act
        var result = await _sut.Create(groupId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(viewResult.ViewName, expectedViewName);
    }

    private static ICollection<Group> GetGroups()
    {

        ICollection<Group> groups = new List<Group>()
        {
            new Group{CourseId = 1, Name = "NewTestGrpoup1", GroupId = 1},
            new Group{CourseId = 1, Name = "NewTestGrpoup2", GroupId = 2},
            new Group{CourseId = 1, Name = "NewTestGrpoup", GroupId = 3},
        };

        return groups;
    }


}
