using Task9.University.Domain.Core;
using Task9.University.Infrastructure.Presentations;

namespace Task9.University.Services.Interfaces;
public interface ICourseService
{
    Task<IEnumerable<Course>> GetAllCourses(CancellationToken cancellationToken);
    Task<EditCourseViewModel> EditCourseView(int id, CancellationToken cancellationToken);
    Task<bool> EditCourse(EditCourseViewModel courseVM, CancellationToken cancellationToken);
    Task<DeleteCourseViewModel> DeleteCourseView(int id, CancellationToken cancellationToken);
    Task<bool> DeleteCourse(DeleteCourseViewModel courseVM, CancellationToken cancellationToken);
    Task<bool> CreateNewCourse(CreateCourseViewModel courseVM, CancellationToken cancellationToken);
}
