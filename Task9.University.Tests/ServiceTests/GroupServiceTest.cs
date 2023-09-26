using Microsoft.Extensions.Logging;
using Moq;
using Task9.University.Infrastructure.Services;
using Task9.University.Services.Interfaces;
using Task9.University.Domain.Core;
using Task9.University.Domain.Interfaces;
using Task9.University.Infrastructure.Presentations;

namespace Task9.University.Tests.ServiceTests;
public class GroupServiceTest
{
    private readonly Mock<IGroupRepository> _groupRepository;
    private readonly Mock<IStudentRepository> _studentRepository;
    private readonly Mock<ICourseRepository> _courseRepository;
    private readonly Mock<ILogger<GroupService>> _logger;
    private readonly IGroupService _sut;
    private readonly CancellationToken _cancellationToken;

    public GroupServiceTest()
    {
        _groupRepository = new Mock<IGroupRepository>();
        _courseRepository = new Mock<ICourseRepository>();
        _studentRepository = new Mock<IStudentRepository>();
        _logger = new Mock<ILogger<GroupService>>();
        _sut = new GroupService(_courseRepository.Object, _groupRepository.Object, _studentRepository.Object, _logger.Object);
    }

    [Fact]
    public async Task GroupService_CreateGroup_ShouldReturnTrueOnSuccess()
    {
        //Arrange
        int testGroupId = 1;
        var testGroup = GetGroups().FirstOrDefault(g => g.GroupId == testGroupId);
        var testCourse = new Course
        {
            CourseId = 1,
            Description = "Test",
            Name = "Test",
        };
        var tetstGroupVM = new CreateGroupViewModel
        {
            Name = "Test",
            CourseId = 1
        };
        _courseRepository.Setup(c => c.GetCourseByIdAsync(testGroup.GroupId, _cancellationToken)).ReturnsAsync(testCourse);
        _groupRepository.Setup(g => g.CreateAsync(It.IsAny<Group>(), _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.CreateGroup(tetstGroupVM, _cancellationToken);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GroupService_CreateGroupView_ShouldReturnViewModel()
    {
        //Arrange
        int testCourseId = 5;
        var testCourse = new Course { CourseId = testCourseId };
        _courseRepository.Setup(x=>x.GetCourseByIdAsync(testCourseId, _cancellationToken)).ReturnsAsync(testCourse);
        //Act
        var result = await _sut.CreateGroupView(testCourseId, _cancellationToken);

        //Assert
        Assert.Equal(testCourseId, result.CourseId);
    }

    [Fact]
    public async Task GroupService_DeleteGroupView_ShouldReturnViewModel()
    {
        //Arrange
        int testGroupId = 1;
        var testGroup = GetGroups().FirstOrDefault(g => g.GroupId == testGroupId);
        _groupRepository.Setup(x => x.GetGroupByIdAsync(testGroupId, _cancellationToken)).ReturnsAsync(testGroup);

        //Act
        var result = await _sut.DeleteGroupView(testGroupId, _cancellationToken);

        //Asert
        Assert.Equal(testGroup.Name, result.Name);
        Assert.Equal(testGroup.GroupId, result.Id);
    }

    [Fact]
    public async Task GroupService_EditGroupView_ShouldReturnViewModel()
    {
        //Arrange
        int testGroupId = 1;
        var testGroup = GetGroups().FirstOrDefault(x => x.GroupId == testGroupId);
        _groupRepository.Setup(x => x.GetGroupByIdAsync(testGroupId, _cancellationToken)).ReturnsAsync(testGroup);

        //Act
        var result = await _sut.EditGroupView(testGroupId, _cancellationToken);

        //Assert
        Assert.Equal(testGroup.Name, result.Name);
        Assert.Equal(testGroup.CourseId, result.CourseId);
    }

    [Fact]
    public async Task GroupService_GroupDelete_ShouldReturnTrueOnSuccess()
    {
        //Arrange
        int testGroupId = 1;
        var testGroup = GetGroups().FirstOrDefault(x => x.GroupId == testGroupId);
        var groupVM = new DeleteGroupViewModel
        {
            Name = testGroup.Name,
            CourseId = (int)testGroup.CourseId,
            Id = testGroup.GroupId
        };

        _groupRepository.Setup(x => x.GetGroupByIdAsync(testGroupId, _cancellationToken)).ReturnsAsync(testGroup);
        _studentRepository.Setup(x => x.GetListByIdAsync(testGroupId, _cancellationToken)).ReturnsAsync(new List<Student>());
        _groupRepository.Setup(x => x.DeleteAsync(It.IsAny<Group>(), _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.GroupDelete(groupVM, _cancellationToken);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GroupService_GroupEdit_ShouldReturnTrueOnSuccess()
    {
        //Arrnage
        int testGroupId = 1;
        var testGroup = GetGroups().FirstOrDefault(x => x.GroupId == testGroupId);
        var groupVM = new EditGroupViewModel
        {
            Name = testGroup.Name,
            CourseId = (int)testGroup.CourseId,
            Id = testGroup.GroupId
        };
        _groupRepository.Setup(x => x.GetGroupByIdNoTrackAsync(testGroupId, _cancellationToken)).ReturnsAsync(testGroup);
        _groupRepository.Setup(x => x.EditAsync(It.IsAny<Group>(), _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.GroupEdit(groupVM, _cancellationToken);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task GroupService_Index_ShouldReturnGroupList()
    {
        //Arrange
        int courseTestId = 1;
        var groupList = GetGroups().Where(g => g.CourseId == courseTestId).ToList();
        _groupRepository.Setup(x => x.GetListByIdAsync(courseTestId, _cancellationToken)).ReturnsAsync(groupList);

        //Act
        var result = await _sut.Index(courseTestId, _cancellationToken);

        //Assert
        Assert.Equal(3, result.Count());
    }


    private static ICollection<Group> GetGroups()
    {

        ICollection<Group> groups = new List<Group>()
        {
            new Group{CourseId = 1, Name = "NewTestGrpoup1", GroupId = 1},
            new Group{CourseId = 1, Name = "NewTestGrpoup2", GroupId = 2},
            new Group{CourseId = 1, Name = "NewTestGrpoup3", GroupId = 3},
        };

        return groups;
    }
}
