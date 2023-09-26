using Microsoft.EntityFrameworkCore;
using System.Threading;
using Task9.University.Domain.Core;
using Task9.University.Domain.Interfaces;

namespace Task9.University.Infrastructure.Data.Repos;
public class GroupRepository : IGroupRepository
{
    private readonly AppDbContext _context;

    public GroupRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<bool> CreateAsync(Group group, CancellationToken cancellationToken)
    {
        await _context.Groups.AddAsync(group, cancellationToken);
        return await SaveAsync(cancellationToken);
    }

    public async Task<bool> DeleteAsync(Group group, CancellationToken cancellationToken)
    {
        _context.Groups.Remove(group);
        return await SaveAsync(cancellationToken);
    }

    public async Task<bool> EditAsync(Group group, CancellationToken cancellationToken)
    {
        _context.Groups.Update(group);
        return await SaveAsync(cancellationToken);
    }

    public async Task<Group> GetGroupByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Groups.FirstOrDefaultAsync(g => g.GroupId == id, cancellationToken);

    }

    public async Task<Group> GetGroupByIdNoTrackAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Groups.AsNoTracking().FirstOrDefaultAsync(g => g.GroupId == id, cancellationToken);

    }

    public async Task<ICollection<Group>> GetListByIdAsync(int id, CancellationToken cancellationToken)
    {
        return await _context.Groups.Where(g => g.CourseId == id).ToListAsync(cancellationToken);
    }

    public async Task<bool> SaveAsync(CancellationToken cancellationToken)
    {
        var saved = _context.SaveChangesAsync(cancellationToken);
        return await saved > 0;
    }
}
