using Task9.University.Domain.Core;

namespace Task9.University.Domain.Interfaces;
public interface ICourseRepository
{
    Task<ICollection<Course>> GetCourseListAsync(CancellationToken cancellationToken);
    Task<Course> GetCourseByIdNoTrackAsync(int id, CancellationToken cancellationToken);
    Task<Course> GetCourseByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> CreateAsync(Course course, CancellationToken cancellationToken);
    Task<bool> EditAsync(Course course, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Course course, CancellationToken cancellationToken);
    Task<bool> SaveAsync(CancellationToken cancellationToken);
}
