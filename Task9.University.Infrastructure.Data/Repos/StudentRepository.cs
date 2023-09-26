using Microsoft.EntityFrameworkCore;
using System.Threading;
using Task9.University.Domain.Core;
using Task9.University.Domain.Interfaces;

namespace Task9.University.Infrastructure.Data.Repos;
public class StudentRepository : IStudentRepository
{
    private readonly AppDbContext _context;

    public StudentRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(Student student, CancellationToken cancellationToken)
    {
        await _context.Students.AddAsync(student, cancellationToken);
        return await SaveAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Student student, CancellationToken cancellationToken)
    {
        _context.Students.Remove(student);
        return await SaveAsync(cancellationToken);
    }

    public async Task<bool> EditAsync(Student student, CancellationToken cancellationToken)
    {
        _context.Students.Update(student);
        return await SaveAsync(cancellationToken);
    }

    public async Task<ICollection<Student>> GetListByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Students.Where(s => s.GroupId == id).ToListAsync(cancellationToken);
    }

    public async Task<Student> GetStudentByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Students.FirstOrDefaultAsync(s => s.StudentId == id, cancellationToken);
    }

    public async Task<Student> GetStudentByIdNoTrackAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Students.AsNoTracking().FirstOrDefaultAsync(s => s.StudentId == id, cancellationToken);
    }

    public async Task<bool> SaveAsync(CancellationToken cancellationToken)
    {
        var saved = await _context.SaveChangesAsync(cancellationToken);
        return saved > 0;
    }
}
