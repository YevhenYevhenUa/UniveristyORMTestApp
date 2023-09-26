using Microsoft.Extensions.Logging;
using Task9.University.Services.Interfaces;
using Task9.University.Domain.Core;
using Task9.University.Domain.Interfaces;
using Task9.University.Infrastructure.Presentations;
using System.ComponentModel.DataAnnotations;
using System.Reflection.Metadata.Ecma335;

namespace Task9.University.Infrastructure.Services;
public class GroupService : IGroupService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly IStudentRepository _studentRepository;
    private readonly ILogger<GroupService> _logger;

    public GroupService(ICourseRepository courseRepository, IGroupRepository groupRepository, 
        IStudentRepository studentRepository, ILogger<GroupService> logger)
    {
        _courseRepository = courseRepository;
        _groupRepository = groupRepository;
        _studentRepository = studentRepository;
        _logger = logger;
    }

    public async Task<bool> CreateGroup(CreateGroupViewModel groupVM, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetCourseByIdAsync((int)groupVM.CourseId, cancellationToken);
        var newGroup = new Group
        {
            Name = groupVM.Name,
            CourseId = groupVM.CourseId,
            Course = course,
            Students = new List<Student>()
        };

        _logger.LogInformation("New group has been successfully created in course number {0}", course.CourseId);
        return await _groupRepository.CreateAsync(newGroup, cancellationToken);
    }


    public async Task<CreateGroupViewModel> CreateGroupView(int id, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetCourseByIdAsync(id, cancellationToken);

        if(course is null)
        {
            return null;
        }

        var newGroupVM = new CreateGroupViewModel
        {
            CourseId = id
        };

        _logger.LogInformation("Triggered create group action in course number {0}", id);
        return newGroupVM;
    }

    public async Task<DeleteGroupViewModel> DeleteGroupView(int id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetGroupByIdAsync(id, cancellationToken);
        if (group is null)
        {
            _logger.LogInformation("");
            return null;
        }

        var groupVM = new DeleteGroupViewModel
        {
            Name = group.Name,
            CourseId = (int)group.CourseId,
            Id = group.GroupId
        };

        _logger.LogInformation("group number {0} is selected for deletion", id);
        return groupVM;
    }

    public async Task<EditGroupViewModel> EditGroupView(int id, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetGroupByIdAsync(id, cancellationToken);
        if (group is null)
        {
            _logger.LogInformation("group not found for modification");
            return null;
        }

        var groupVM = new EditGroupViewModel
        {
            Name = group.Name,
            CourseId = (int)group.CourseId,
            Id = group.GroupId
        };

        _logger.LogInformation("group number {0} is selected for modification", id);
        return groupVM;
    }

    public async Task<bool> GroupDelete(DeleteGroupViewModel groupVM, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetGroupByIdAsync(groupVM.Id, cancellationToken);
        if (group is null)
        {
            _logger.LogInformation("The group was not found when attempting to delete");
        }

        var studentList = await _studentRepository.GetListByIdAsync(group.GroupId, new CancellationToken());
        if (studentList.Count != 0)
        {
            return false;
        }

        _logger.LogInformation("Group number {0} successfully deleted", group.GroupId);
        return await _groupRepository.DeleteAsync(group, cancellationToken);
    }

    public async Task<bool> GroupEdit(EditGroupViewModel groupVM, CancellationToken cancellationToken)
    {
        var group = await _groupRepository.GetGroupByIdNoTrackAsync(groupVM.Id, cancellationToken);
        if (group is null)
        {
            _logger.LogInformation("Something went wrong while editing group number {0}", group.GroupId);
            return false;
        }

        var newGroup = new Group
        {
            Course = group.Course,
            CourseId = group.CourseId,
            GroupId = group.GroupId,
            Name = groupVM.Name,
            Students = group.Students

        };

        _logger.LogInformation("Group number {0} successfully edited", groupVM.Id);
        return await _groupRepository.EditAsync(newGroup, cancellationToken);
    }

    public async Task<IEnumerable<Group>> Index(int id, CancellationToken cancellationToken)
    {
        var groupList = await _groupRepository.GetListByIdAsync(id, cancellationToken);
        if (groupList.Count == 0)
        {
            groupList.Add(new Group { CourseId = id });
        }

        _logger.LogInformation("From database returned {0} groups", groupList.Count);
        return groupList;
    }
}
