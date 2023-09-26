using Task9.University.Domain.Core;
using Task9.University.Infrastructure.Presentations;

namespace Task9.University.Services.Interfaces;
public interface IStudentService
{
    Task<IEnumerable<Student>> Index(int id, CancellationToken cancellationToken);
    Task<EditStudentViewModel> EditStudentView(int id, CancellationToken cancellationToken);
    Task<bool> EditStudent(EditStudentViewModel studentVM, CancellationToken cancellationToken);
    Task<DeleteStudentViewModel> DeleteStudentView(int id, CancellationToken cancellationToken);
    Task<bool> DeleteStudent(DeleteStudentViewModel studentVM, CancellationToken cancellationToken);
    Task<CreateStudentViewModel> CreateStudentView(int id, CancellationToken cancellationToken);
    Task<bool> CreateNewStudent(CreateStudentViewModel studentVM, CancellationToken cancellationToken);
}
