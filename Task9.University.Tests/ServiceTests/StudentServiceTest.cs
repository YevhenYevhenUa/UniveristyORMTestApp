using Microsoft.Extensions.Logging;
using Moq;
using Task9.University.Infrastructure.Services;
using Task9.University.Services.Interfaces;
using Task9.University.Domain.Core;
using Task9.University.Domain.Interfaces;
using Task9.University.Infrastructure.Presentations;

namespace Task9.University.Tests.ServiceTests;
public class StudentServiceTest
{
    private readonly IStudentService _sut;
    private readonly Mock<IStudentRepository> _studentRepository;
    private readonly Mock<IGroupRepository> _groupRepository;
    private readonly Mock<ILogger<StudentService>> _logger;
    private readonly CancellationToken _cancellationToken;
    public StudentServiceTest()
    {
        _groupRepository = new Mock<IGroupRepository>();
        _studentRepository = new Mock<IStudentRepository>();
        _logger = new Mock<ILogger<StudentService>>();
        _sut = new StudentService(_studentRepository.Object, _groupRepository.Object, _logger.Object);
        _cancellationToken = new CancellationToken();
    }
    [Fact]
    public async Task StudentService_CreateNewStudent_ShouldReturnTrueWhenSucces()
    {
        //Arrange
        int studentId = 1;
        var studentTest = GetStudents().FirstOrDefault(s => s.StudentId == studentId);
        var testStudent = new CreateStudentViewModel
        {
            FirstName = studentTest.FirstName,
            LastName = studentTest.LastName,
            GroupId = studentTest.GroupId,
        };

        _groupRepository.Setup(x => x.GetGroupByIdAsync((int)testStudent.GroupId, _cancellationToken)).ReturnsAsync(new Group
        {
            Name = "test",
            CourseId = 1,
            GroupId = 1
        });

        _studentRepository.Setup(x => x.CreateAsync(It.IsAny<Student>(), _cancellationToken)).ReturnsAsync(true);


        //Act
        var result = await _sut.CreateNewStudent(testStudent, _cancellationToken);

        //Assert
        Assert.True(result);
    }
    [Fact]
    public async Task StudentService_CreateNewStudentView_ShoulReturnViewModel()
    {
        //Arrange
        var testGrouId = 5;
        var testGroup = new Group { GroupId = testGrouId };
        _groupRepository.Setup(x => x.GetGroupByIdAsync(testGrouId, _cancellationToken)).ReturnsAsync(testGroup);

        //Act
        var result = await _sut.CreateStudentView(testGrouId, _cancellationToken);

        //Assert
        Assert.Equal(testGrouId, result.GroupId);
    }

    [Fact]
    public async Task StudentService_DeleteStudent_ShouldReturnTrueOnSuccess()
    {
        //Arrange
        var testStudentVM = new DeleteStudentViewModel
        {
            FirstName = "TestName",
            LastName = "TestName2",
            GroupId = 1,
            Id = 5
        };

        int studentId = 1;
        var studentTest = GetStudents().FirstOrDefault(s => s.StudentId == studentId);
        _studentRepository.Setup(x => x.GetStudentByIdAsync(testStudentVM.Id, _cancellationToken)).ReturnsAsync(studentTest);

        _studentRepository.Setup(x => x.DeleteAsync(It.IsAny<Student>(), _cancellationToken)).ReturnsAsync(true);

        //Act
        var result = await _sut.DeleteStudent(testStudentVM, _cancellationToken);

        //Assert
        Assert.True(result);
    }
    [Fact]
    public async Task StudentService_DeleteStudentView_ShouldReturnViewModel()
    {
        //Arrange
        int testStudentId = 1;

        var testStudent = GetStudents().FirstOrDefault(s => s.StudentId == testStudentId);
        ; _studentRepository.Setup(x => x.GetStudentByIdAsync(testStudentId, _cancellationToken)).ReturnsAsync(testStudent);

        //Act
        var result = await _sut.DeleteStudentView(testStudentId, _cancellationToken);

        //Assert
        Assert.Equal(testStudent.FirstName, result.FirstName);
        Assert.Equal(testStudent.LastName, result.LastName);
    }

    [Fact]
    public async Task StudentService_EditStudent_ShouldReturnTrueOnSuccess()
    {
        //Arrange
        var testStudentVM = new EditStudentViewModel
        {
            FirstName = "testFirst",
            LastName = "testLast",
            GroupId = 1,
        };
        int studentId = 1;
        var testStudent = GetStudents().FirstOrDefault(s => s.StudentId == studentId);

        _studentRepository.Setup(x => x.GetStudentByIdNoTrackAsync(testStudentVM.Id, _cancellationToken)).ReturnsAsync(testStudent);
        _studentRepository.Setup(x => x.EditAsync(It.IsAny<Student>(), _cancellationToken)).ReturnsAsync(true);
        //Act
        var result = await _sut.EditStudent(testStudentVM, _cancellationToken);

        //Assert
        Assert.True(result);
    }

    [Fact]
    public async Task StudentService_EditStudentView_ShouldReturnViewModel()
    {
        //Arrange
        int testStudentId = 1;
        var testStudent = GetStudents().FirstOrDefault(s => s.StudentId == testStudentId);

        _studentRepository.Setup(x => x.GetStudentByIdAsync(testStudentId, _cancellationToken)).ReturnsAsync(testStudent);

        //Act
        var result = await _sut.EditStudentView(testStudentId, _cancellationToken);

        //Assert
        Assert.Equal(testStudent.FirstName, result.FirstName);
        Assert.Equal(testStudent.LastName, result.LastName);
    }

    [Fact]
    public async Task StudentService_Index_ShouldReturnListOfStudent()
    {
        //Arrange
        int testGroupId = 1;
        var studentList = GetStudents();

        _studentRepository.Setup(x => x.GetListByIdAsync(testGroupId, _cancellationToken)).ReturnsAsync(studentList);

        //Act
        var result = await _sut.Index(testGroupId, _cancellationToken);

        //Assert
        Assert.Equal(2, result.Count());

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
