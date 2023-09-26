using Microsoft.AspNetCore.Mvc;
using Moq;
using Task9.University.Services.Interfaces;
using Task9.University.Domain.Core;
using Task9.University.Infrastructure.Presentations;
using Task9University.Controllers;

namespace Task9.University.Tests.ControllersTests;

public class StudentsControllerTest
{

    private readonly Mock<IStudentService> _studentServices;
    private readonly StudentsController _sut;
    private readonly CancellationToken _cancellationToken;
    public StudentsControllerTest()
    {
        _studentServices = new Mock<IStudentService>();
        _sut = new StudentsController(_studentServices.Object);
        _cancellationToken = new CancellationToken();
    }

    [Fact]
    public async Task StudentsContollers_Index_ShouldReturnListOfStudent()
    {
        //Arrange
        int groupId = 1;
        var studentList = GetStudents();
        _studentServices.Setup(x => x.Index(groupId, _cancellationToken)).ReturnsAsync(studentList);

        //Act
        var result = await _sut.Index(groupId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<IEnumerable<Student>>(viewResult.ViewData.Model);
        Assert.Equal(2, model.Count());

    }

    [Fact]
    public async Task StudentController_Index_ShouldReturnErrorViewIfNull()
    {
        //Arrange
        int groupId = 5;
        string expectedViewName = "Error";
        _studentServices.Setup(x => x.Index(groupId, _cancellationToken)).ReturnsAsync(() => null);

        //Act
        var result = await _sut.Index(groupId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(viewResult.ViewName, expectedViewName);
    }

    [Fact]
    public async Task StudentContollers_Edit_ShuldReturnEditViewModelForSelectedStudent()
    {
        //Arrange
        int studentId = 1;
        var student = GetStudents().FirstOrDefault(s => s.StudentId == studentId);
        var studentMV = new EditStudentViewModel
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            GroupId = (int)student.GroupId,
            Id = student.StudentId
        };
        _studentServices.Setup(x => x.EditStudentView(studentId, _cancellationToken)).ReturnsAsync(studentMV);

        //Act
        var result = await _sut.Edit(studentId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<EditStudentViewModel>(viewResult.ViewData.Model);
        Assert.Equal(student.FirstName, model.FirstName);
        Assert.Equal(student.LastName, model.LastName);
    }

    [Fact]
    public async Task StudentController_Edit_ShouldReturnErrorViewIfNull()
    {
        //Arrange
        int studentId = 5;
        string expectedViewName = "Error";
        _studentServices.Setup(x => x.EditStudentView(studentId, _cancellationToken)).ReturnsAsync(() => null);

        //Act
        var result = await _sut.Edit(studentId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(viewResult.ViewName, expectedViewName);
    }

    [Fact]
    public async Task StudentControllers_Edit_ShouldReturnRedirectToRoute()
    {
        //Arrange
        int studentId = 1;
        string expectedAction = "Index";
        var student = GetStudents().FirstOrDefault(s => s.StudentId == studentId);
        var studentMV = new EditStudentViewModel
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            GroupId = (int)student.GroupId,
            Id = student.StudentId
        };
        _studentServices.Setup(x => x.EditStudent(studentMV, _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.Edit(studentMV, _cancellationToken) as RedirectToRouteResult;

        //Assert
        Assert.Equal(expectedAction, result.RouteValues["action"]);
    }

    [Fact]
    public async Task StudentController_Edit_ShouldReturnErrorView()
    {
        //Arrange
        int studentId = 999;
        string expectView = "Error";
        _studentServices.Setup(x => x.EditStudentView(studentId, _cancellationToken)).ReturnsAsync(() => null);

        //Act
        var result = await _sut.Edit(studentId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(expectView, viewResult.ViewName);
    }


    [Fact]
    public async Task StudentController_Delete_ShouldReturnStudent()
    {
        //Arrange
        int studentId = 1;
        var student = GetStudents().FirstOrDefault(s => s.StudentId == studentId);
        var studentVM = new DeleteStudentViewModel
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            GroupId = (int)student.GroupId,
            Id = student.StudentId
        };
        _studentServices.Setup(x => x.DeleteStudentView(studentId, _cancellationToken)).ReturnsAsync(studentVM);

        //Act
        var result = await _sut.Delete(studentId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<DeleteStudentViewModel>(viewResult.ViewData.Model);
        Assert.Equal(student.FirstName, model.FirstName);
        Assert.Equal(student.LastName, model.LastName);
    }

    [Fact]
    public async Task StudentController_Delete_ShouldRedirectToRoute()
    {
        //Arrange
        string expectAction = "Index";
        int studentId = 1;
        var student = GetStudents().FirstOrDefault(s => s.StudentId == studentId);
        var studentVM = new DeleteStudentViewModel
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            GroupId = (int)student.GroupId,
            Id = student.StudentId
        };
        _studentServices.Setup(x => x.DeleteStudent(studentVM, _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.Delete(studentVM, _cancellationToken) as RedirectToRouteResult;

        //Assert
        Assert.Equal(expectAction, result.RouteValues["Action"]);
    }

    [Fact]
    public async Task StudentController_Delete_ShouldReturnErrorViewIfNull()
    {
        //Arrange
        int studentId = 5;
        string expectedViewName = "Error";
        _studentServices.Setup(x => x.DeleteStudentView(studentId, _cancellationToken)).ReturnsAsync(() => null);

        //Act
        var result = await _sut.Delete(studentId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(viewResult.ViewName, expectedViewName);
    }

    [Fact]
    public async Task StudentController_Create_ShouldReturnCreateStudentViewModel()
    {
        //Arrange
        int stundentId = 1;
        var studentVM = new CreateStudentViewModel
        {
            FirstName = "TestName",
            LastName = "TestName",
            GroupId = 1
        };
        _studentServices.Setup(x => x.CreateStudentView(stundentId, _cancellationToken)).ReturnsAsync(studentVM);

        //Act
        var result = await _sut.Create(stundentId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        var model = Assert.IsAssignableFrom<CreateStudentViewModel>(viewResult.ViewData.Model);
        Assert.Equal(stundentId, model.GroupId);
        Assert.Equal("TestName", model.FirstName);
    }

    [Fact]
    public async Task StudentController_Create_ShouldRedirectToRoute()
    {
        //Arrange
        string expectAction = "Index";
        var testData = new CreateStudentViewModel { FirstName = "TestData", LastName = "TestData", GroupId = 1 };
        _studentServices.Setup(x => x.CreateNewStudent(testData, _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.Create(testData, _cancellationToken) as RedirectToRouteResult;

        //Assert
        Assert.Equal(expectAction, result.RouteValues["Action"]);
    }

    [Fact]
    public async Task StudentController_Create_ShouldReturnErrorViewIfNull()
    {
        //Arrange
        int studentId = 5;
        string expectedViewName = "Error";
        _studentServices.Setup(x => x.CreateStudentView(studentId, _cancellationToken)).ReturnsAsync(() => null);

        //Act
        var result = await _sut.Create(studentId, _cancellationToken);

        //Assert
        var viewResult = Assert.IsType<ViewResult>(result);
        Assert.Equal(viewResult.ViewName, expectedViewName);
    }

    private static ICollection<Student> GetStudents()
    {
        ICollection<Student> students = new List<Student>
        {
            new Student {FirstName = "TestData", LastName ="TestData", GroupId = 1, StudentId = 1 },
            new Student {FirstName = "TestData", LastName ="TestData", GroupId = 1, StudentId = 2 }
        };

        return students;
    }
}

