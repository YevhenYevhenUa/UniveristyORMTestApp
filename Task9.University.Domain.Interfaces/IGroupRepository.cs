using Task9.University.Domain.Core;

namespace Task9.University.Domain.Interfaces;
public interface IGroupRepository
{
    Task<ICollection<Group>> GetListByIdAsync(int id, CancellationToken cancellationToken);
    Task<Group> GetGroupByIdNoTrackAsync(int id, CancellationToken cancellationToken);
    Task<Group> GetGroupByIdAsync(int id, CancellationToken cancellationToken);
    Task<bool> CreateAsync(Group group, CancellationToken cancellationToken);
    Task<bool> EditAsync(Group group, CancellationToken cancellationToken);
    Task<bool> DeleteAsync(Group group, CancellationToken cancellationToken);
    Task<bool> SaveAsync(CancellationToken cancellationToken);
}
