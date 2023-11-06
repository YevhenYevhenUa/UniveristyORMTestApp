using Microsoft.AspNetCore.Mvc;
using Moq;
using Task9.University.Services.Interfaces;
using Task9.University.Domain.Core;
using Task9.University.Infrastructure.Presentations;
using Task9University.Controllers;
using Task9.University.Infrastructure.Services;

namespace Task9.University.Tests.ControllersTests;
public class HomeControllerTests
{
    private readonly Mock<ICourseService> _courseService;
    private readonly HomeController _sut;
    private readonly CancellationToken _cancellationToken;

    public HomeControllerTests()
    {
        _courseService = new Mock<ICourseService>();
        _sut = new HomeController(_courseService.Object);
        _cancellationToken = new CancellationToken();
    }

    [Fact]
    public async Task HomeController_Index_ShouldReturnCourseList()
    {
        //Arrange
        var courses = GetCourses();
        _courseService.Setup(x => x.GetAllCourses(_cancellationToken)).ReturnsAsync(courses);

        //Act
        var result = await _sut.Index(1, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Course>>(viewResult.ViewData.Model);
        Assert.Equal(3, model.Count());
    }
     
    [Fact]
    public async Task HomeController_Edit_ShouldReturnEditCourseViewModel()
    {
        //Arrange
        int courseId = 1;
        var course = GetCourses().FirstOrDefault(c => c.CourseId == courseId);
        var courseVM = new EditCourseViewModel { Description = course.Description, Name = course.Name, Id = course.CourseId };
        _courseService.Setup(x => x.EditCourseView(courseId, _cancellationToken)).ReturnsAsync(courseVM);

        //Act
        var result = await _sut.Edit(courseId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<EditCourseViewModel>(viewResult.ViewData.Model);
        Assert.Equal(course.Name, model.Name);
        Assert.Equal(course.Description, model.Description);
    }

    [Fact]
    public async Task HomeController_Edit_ShouldReturnRedirectionToAction()
    {
        //Arrange
        string expectAction = "Index";
        int courseId = 1;
        var course = GetCourses().FirstOrDefault(c => c.CourseId == courseId);
        var courseVM = new EditCourseViewModel { Description = course.Description, Name = course.Name, Id = course.CourseId };
        _courseService.Setup(x => x.EditCourse(courseVM, _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.Edit(courseVM, _cancellationToken) as RedirectToActionResult;

        //Assert
        Assert.Equal(expectAction, result.ActionName);
    }

    [Fact]
    public async Task HomeController_Edit_ShouldReturnErrorViewIfNull()
    {
        //Arrange
        int courseId = 5;
        string expectedViewName = "Error";
        _courseService.Setup(x => x.EditCourseView(courseId, _cancellationToken)).ReturnsAsync(() => null);

        //Act
        var result = await _sut.Edit(courseId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(viewResult.ViewName, expectedViewName);
    }

    [Fact]
    public async Task HomeController_Delete_ShouldReturnErrorViewIfNull()
    {
        //Arrange
        int courseId = 5;
        string expectedViewName = "Error";
        _courseService.Setup(x => x.DeleteCourseView(courseId, _cancellationToken)).ReturnsAsync(() => null);

        //Act
        var result = await _sut.Delete(courseId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(viewResult.ViewName, expectedViewName);
    }

    [Fact]
    public async Task HomeController_Delete_ShouldReturnCourse()
    {
        //Arrange
        int courseId = 1;
        var course = GetCourses().FirstOrDefault(c => c.CourseId == courseId);
        var courseVM = new DeleteCourseViewModel { Name = course.Name, Description = course.Description, Id = course.CourseId };
        _courseService.Setup(x => x.DeleteCourseView(courseId, _cancellationToken)).ReturnsAsync(courseVM);

        //Act
        var result = await _sut.Delete(courseId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<DeleteCourseViewModel>(viewResult.ViewData.Model);
        Assert.Equal(course.Name, model.Name);
        Assert.Equal(course.Description, model.Description);
    }

    [Fact]
    public async Task HomeController_Delete_ShouldReturnRedirectToAction()
    {
        //Arrange
        string expectAction = "Index";
        int courseId = 1;
        var course = GetCourses().FirstOrDefault(c => c.CourseId == courseId);
        var courseVM = new DeleteCourseViewModel
        {
            Name = course.Name,
            Description = course.Description,
            Id = course.CourseId
        };
        _courseService.Setup(x => x.DeleteCourse(courseVM, _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.Delete(courseVM, _cancellationToken) as RedirectToActionResult;

        //Assert
        Assert.Equal(expectAction, result.ActionName);
    }

    [Fact]
    public async Task HomeController_Create_ShouldReturnViewResult()
    {
        //Arrange
        string expectView = "Create";

        //Act
        var result = await _sut.Create();

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(expectView, viewResult.ViewName);
    }

    [Fact]
    public async Task HomeController_Create_ShouldReturnRedirectionToAction()
    {
        //Arrange
        string expectAction = "Index";
        var testData = new CreateCourseViewModel
        {
            Name = "Test",
            Description = "Test"
        };
        _courseService.Setup(x=>x.CreateNewCourse(testData, _cancellationToken)).ReturnsAsync(true);
        //Act
        var result =  await _sut.Create(testData, _cancellationToken) as RedirectToActionResult;

        //Assert
        Assert.Equal(expectAction, result.ActionName);
    }

    private static ICollection<Course> GetCourses()
    {
        ICollection<Course> courses = new List<Course>()
        {
            new Course{Name = "testName1", Description = "testDescriprion1", CourseId = 1},
            new Course{Name = "testName2", Description = "testDescriprion2", CourseId = 2},
            new Course{Name = "testName3", Description = "testDescriprion3", CourseId = 3},
        };

        return courses;
    }
}
