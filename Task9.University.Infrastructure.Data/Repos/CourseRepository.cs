using Microsoft.EntityFrameworkCore;
using System.Threading;
using Task9.University.Domain.Core;
using Task9.University.Domain.Interfaces;

namespace Task9.University.Infrastructure.Data.Repos;
public class CourseRepository : ICourseRepository
{
    private readonly AppDbContext _context;

    public CourseRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(Course course, CancellationToken cancellationToken)
    {
        await _context.Courses.AddAsync(course, cancellationToken);
        return await SaveAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Course course, CancellationToken cancellationToken)
    {
        _context.Courses.Remove(course);
        return await SaveAsync(cancellationToken);
    }

    public async Task<bool> EditAsync(Course course, CancellationToken cancellationToken)
    {
        _context.Courses.Update(course);
        return await SaveAsync(cancellationToken);
    }

    public async Task<ICollection<Course>> GetCourseListAsync(CancellationToken cancellationToken)
    {
        return await _context.Courses.ToListAsync(cancellationToken);
    }

    public async Task<Course> GetCourseByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Courses.FirstOrDefaultAsync(c => c.CourseId == id, cancellationToken);
    }

    public async Task<Course> GetCourseByIdNoTrackAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Courses.AsNoTracking().FirstOrDefaultAsync(c => c.CourseId == id, cancellationToken);
    }

    public async Task<bool> SaveAsync(CancellationToken cancellationToken)
    {
        var saved = _context.SaveChangesAsync(cancellationToken);
        return await saved > 0;
    }
}
