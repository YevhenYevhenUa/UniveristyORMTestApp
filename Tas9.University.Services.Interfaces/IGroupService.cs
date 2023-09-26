using Task9.University.Domain.Core;
using Task9.University.Infrastructure.Presentations;

namespace Task9.University.Services.Interfaces;
public interface IGroupService
{
    Task<IEnumerable<Group>> Index(int id, CancellationToken cancellationToken);
    Task<EditGroupViewModel> EditGroupView(int id, CancellationToken cancellationToken);
    Task<bool> GroupEdit(EditGroupViewModel groupVM, CancellationToken cancellationToken);
    Task<DeleteGroupViewModel> DeleteGroupView(int id, CancellationToken cancellationToken);
    Task<bool> GroupDelete(DeleteGroupViewModel groupVM, CancellationToken cancellationToken);
    Task<CreateGroupViewModel> CreateGroupView(int id, CancellationToken cancellationToken);
    Task<bool> CreateGroup(CreateGroupViewModel groupVM, CancellationToken cancellationToken);
}
