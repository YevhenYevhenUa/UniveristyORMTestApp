using Microsoft.Extensions.Logging;
using Task9.University.Services.Interfaces;
using Task9.University.Domain.Core;
using Task9.University.Domain.Interfaces;
using Task9.University.Infrastructure.Presentations;

namespace Task9.University.Infrastructure.Services;
public class StudentService : IStudentService
{
    private readonly IStudentRepository _studentRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly ILogger<StudentService> _logger;

    public StudentService(IStudentRepository studentRepository, 
        IGroupRepository groupRepository, ILogger<StudentService> logger)
    {
        _studentRepository = studentRepository;
        _groupRepository = groupRepository;
        _logger = logger;
    }

    public async Task<bool> CreateNewStudent(CreateStudentViewModel studentVM, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetGroupByIdAsync((int)studentVM.GroupId, cancellationToken);
        var newStundet = new Student
        {
            FirstName = studentVM.FirstName,
            LastName = studentVM.LastName,
            GroupId = studentVM.GroupId,
            Group = group,
        };

        _logger.LogInformation("A new student in group {0} has been successfully created", studentVM.GroupId);
        return await _studentRepository.CreateAsync(newStundet, cancellationToken);
    }

    public async Task<CreateStudentViewModel> CreateStudentView(int GroupId, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetGroupByIdAsync(GroupId, cancellationToken);
        if(group is null)
        {
            return null;
        }
        
        var newStudetVM = new CreateStudentViewModel
        {
            GroupId = GroupId
        };

        _logger.LogInformation("Triggered create student action in group {0}", GroupId);
        return newStudetVM;
    }

    public async Task<bool> DeleteStudent(DeleteStudentViewModel studentVM, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetStudentByIdAsync(studentVM.Id, cancellationToken);

        if (student is null)
        {
            _logger.LogInformation("student nubmer {0} not found when attempting to deletion", studentVM.Id);
            return false;
        }

        _logger.LogInformation("Student number {0} from group {1} successfully deleted", studentVM.Id, studentVM.GroupId);
        return await _studentRepository.DeleteAsync(student, cancellationToken);
    }

    public async Task<DeleteStudentViewModel> DeleteStudentView(int id, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetStudentByIdAsync(id, cancellationToken);
        if (student is null)
        {
            _logger.LogInformation("student nubmer {0} not found when attempting to deletion", id);
            return null;
        }

        var studentVM = new DeleteStudentViewModel
        {
            FirstName = student.FirstName,
            LastName = student.LastName,
            Id = student.StudentId,
            GroupId = (int)student.GroupId
        };

        _logger.LogInformation("Student number {0} from group {1} is selected for deletion", id, student.GroupId);
        return studentVM;
    }

    public async Task<bool> EditStudent(EditStudentViewModel studentVM, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetStudentByIdNoTrackAsync(studentVM.Id, cancellationToken);
        if (student is null)
        {
            _logger.LogInformation(message: "Something went wrong while editing student number {0} from group {1}", student.StudentId, student.GroupId);
            return false;
        }

        var newStudent = new Student
        {
            StudentId = student.StudentId,
            FirstName = studentVM.FirstName,
            LastName = studentVM.LastName,
            Group = student.Group,
            GroupId = student.GroupId
        };

        _logger.LogInformation(message: "Student number {0} from group {1} is successfully edited", student.StudentId, student.GroupId);
        return await _studentRepository.EditAsync(newStudent, cancellationToken);

    }

    public async Task<EditStudentViewModel> EditStudentView(int id, CancellationToken cancellationToken)
    {
        var student = await _studentRepository.GetStudentByIdAsync(id, cancellationToken);
        if (student is null)
        {
            _logger.LogInformation("Student nubmer {0} not found when attempting to change", id);
            return null;
        }

        var newStudentVM = new EditStudentViewModel
        {
            Id = student.StudentId,
            GroupId = (int)student.GroupId,
            FirstName = student.FirstName,
            LastName = student.LastName
        };

        _logger.LogInformation(message: "Student number {0} from group {1} is selected for edit", id, student.GroupId);
        return newStudentVM;
    }

    public async Task<IEnumerable<Student>> Index(int id, CancellationToken cancellationToken)
    {
        var studentList = await _studentRepository.GetListByIdAsync(id, cancellationToken);
        if (studentList.Count == 0)
        {
            studentList.Add(new Student { GroupId = id });
        }

        _logger.LogInformation(message: "Returned from database and returned to the view {0} students from group {1}", studentList.Count, studentList.First().GroupId);

        return studentList;
    }
}
