using Task9.University.Domain.Core;

namespace Task9.University.Domain.Interfaces;
public interface IStudentRepository
{
    Task<ICollection<Student>> GetListByIdAsync(int id, CancellationToken cancellationToken);
    Task<Student> GetStudentByIdNoTrackAsync(int id, CancellationToken cancellationToken);
    Task<Student> GetStudentByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> CreateAsync(Student student, CancellationToken cancellationToken);
    Task<bool> EditAsync(Student student, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Student student, CancellationToken cancellationToken);
    Task<bool> SaveAsync(CancellationToken cancellationToken);
}
