using Microsoft.Extensions.Logging;
using Moq;
using Task9.University.Infrastructure.Services;
using Task9.University.Domain.Core;
using Task9.University.Domain.Interfaces;
using Task9.University.Infrastructure.Presentations;

namespace Task9.University.Tests.ServiceTests;
public class CourseServiceTests
{
    private readonly Mock<ICourseRepository> _courseRepository;
    private readonly Mock<IGroupRepository> _groupRepository;
    private readonly Mock<ILogger<CourseService>> _logger;
    private readonly CourseService _sut;
    private readonly CancellationToken _cancellationToken;

    public CourseServiceTests()
    {
        _courseRepository = new Mock<ICourseRepository>();
        _groupRepository = new Mock<IGroupRepository>();
        _logger = new Mock<ILogger<CourseService>>();
        _sut = new CourseService(_courseRepository.Object, _groupRepository.Object, _logger.Object);
        _cancellationToken = new CancellationToken();
    }

    [Fact]
    public async Task CourseService_CreateNewCourse_ReturnTrueOnSuccess()
    {
        //Arrange
        var testCourseVM = new CreateCourseViewModel
        {
            Description = "Test",
            Name = "Test"
        };
        _courseRepository.Setup(x => x.CreateAsync(It.IsAny<Course>(), _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.CreateNewCourse(testCourseVM, _cancellationToken);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CourseService_DeleteCourse_ShouldReturnTrueOnSuccess()
    {
        //Arrange
        int courseTestId = 1;
        var courseTest = GetCourses().FirstOrDefault(c=>c.CourseId == courseTestId);    
        var courseVM = new DeleteCourseViewModel
        {
            Description= "Test",
            Name = "Test",
            Id = courseTestId
        };

        _courseRepository.Setup(x => x.GetCourseByIdAsync(courseTestId, _cancellationToken)).ReturnsAsync(courseTest);
        _groupRepository.Setup(x => x.GetListByIdAsync(courseTestId, _cancellationToken)).ReturnsAsync(new List<Group>());
        _courseRepository.Setup(x=>x.DeleteAsync(courseTest, _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.DeleteCourse(courseVM, _cancellationToken);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CourseService_DeletCourseView_ShouldRetunrViewModel()
    {
        //Arrange
        int courseTestId = 1;
        var courseTest = GetCourses().FirstOrDefault(c => c.CourseId == courseTestId);
        _courseRepository.Setup(x => x.GetCourseByIdAsync(courseTestId, _cancellationToken)).ReturnsAsync(courseTest);

        //Act
        var result = await _sut.DeleteCourseView(courseTestId, _cancellationToken);

        //Assert
        Assert.Equal(courseTest.Name, result.Name);
        Assert.Equal(courseTest.Description, result.Description);
    }

    [Fact]
    public async Task CourseService_EditCourse_ReturnTrueOnSuccess()
    {
        //Arrange
        int courseTestId = 1;
        var courseTest = GetCourses().FirstOrDefault(c => c.CourseId == courseTestId);
        _courseRepository.Setup(x => x.GetCourseByIdNoTrackAsync(courseTestId, _cancellationToken)).ReturnsAsync(courseTest);
        var courseVM = new EditCourseViewModel
        {
            Description = "Test1",
            Name = "Test1",
            Id = 1
        };
        _courseRepository.Setup(x => x.EditAsync(It.IsAny<Course>(), _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.EditCourse(courseVM, _cancellationToken);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task CourseService_EditCourseView_ReturnViewModel()
    {
        //Arrange
        int courseTestId = 1;
        var courseTest = GetCourses().FirstOrDefault(c => c.CourseId == courseTestId);
        _courseRepository.Setup(x => x.GetCourseByIdAsync(courseTestId, _cancellationToken)).ReturnsAsync(courseTest);

        //Act
        var result = await _sut.EditCourseView(courseTestId, _cancellationToken);

        //Assert
        Assert.Equal(courseTest.Name, result.Name);
        Assert.Equal(courseTest.Description, result.Description);
    }

    [Fact]
    public async Task CourseService_GetAllCourses_ReturnCourseList()
    {
        //Arrange
        var courseList = GetCourses();
        _courseRepository.Setup(x=>x.GetCourseListAsync(_cancellationToken)).ReturnsAsync(courseList);

        //Act
        var result = await _sut.GetAllCourses(_cancellationToken);

        //Assert
        Assert.Equal(3, result.Count());
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
