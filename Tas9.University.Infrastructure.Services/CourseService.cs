using Microsoft.Extensions.Logging;
using Task9.University.Services.Interfaces;
using Task9.University.Domain.Core;
using Task9.University.Domain.Interfaces;
using Task9.University.Infrastructure.Presentations;

namespace Task9.University.Infrastructure.Services;
public class CourseService : ICourseService
{
    private readonly ICourseRepository _courseRepository;
    private readonly IGroupRepository _groupRepository;
    private readonly ILogger<CourseService> _logger;

    public CourseService(ICourseRepository courseRepository, IGroupRepository groupRepository,
        ILogger<CourseService> logger)
    {
        _courseRepository = courseRepository;
        _groupRepository = groupRepository;
        _logger = logger;
    }

    public async Task<bool> CreateNewCourse(CreateCourseViewModel courseVM, CancellationToken cancellationToken)
    {
        var newCourse = new Course
        {
            Description = courseVM.Description,
            Name = courseVM.Name,
        };

        _logger.LogInformation("New course has been successfully created");
        return await _courseRepository.CreateAsync(newCourse, cancellationToken);
    }

    public async Task<bool> DeleteCourse(DeleteCourseViewModel courseVM, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetCourseByIdAsync(courseVM.Id, cancellationToken);
        if (course is null)
        {
            _logger.LogInformation("course number {0} not found when attempting to delete", courseVM.Id);
            return false;
        }

        var groupList = await _groupRepository.GetListByIdAsync(course.CourseId, cancellationToken);
        if (groupList.Count != 0)
        {
            _logger.LogInformation("Course number {0} successfully deleted", courseVM.Id);
            return false;
        }

        _logger.LogInformation("Course number {0} successfully deleted", courseVM.Id);
        return await _courseRepository.DeleteAsync(course, cancellationToken);
    }

    public async Task<DeleteCourseViewModel> DeleteCourseView(int id, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetCourseByIdAsync(id, cancellationToken);
        if (course is null)
        {
            _logger.LogInformation("course number {0} not found when attempting to delete", id);
            return null;
        }

        var courseVM = new DeleteCourseViewModel
        {
            Id = course.CourseId,
            Name = course.Name,
            Description = course.Description
        };

        _logger.LogInformation("Course number {0} is selected for deletion", id);
        return courseVM;
    }

    public async Task<bool> EditCourse(EditCourseViewModel courseVM, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetCourseByIdNoTrackAsync(courseVM.Id, cancellationToken);
        if (course is null)
        {
            _logger.LogInformation("Something went wrong while editing course number {0}", courseVM.Id);
            return false;
        }

        var newCourse = new Course
        {
            Name = courseVM.Name,
            Description = courseVM.Description,
            CourseId = course.CourseId,
            Groups = course.Groups
        };

        _logger.LogInformation("Course number {0} successfully edited!", course.CourseId);
        return await _courseRepository.EditAsync(newCourse, cancellationToken);
    }

    public async Task<EditCourseViewModel> EditCourseView(int id, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetCourseByIdAsync(id, cancellationToken);
        if (course is null)
        {
            _logger.LogInformation("course number {0} not found when attempting to change", id);
            return null;
        }

        var newCourseVM = new EditCourseViewModel
        {
            Name = course.Name,
            Description = course.Description,
            Id = course.CourseId
        };

        _logger.LogInformation("Course number {0} is selected for editing", id);
        return newCourseVM;
    }

    public async Task<IEnumerable<Course>> GetAllCourses(CancellationToken cancellationToken)
    {
        var courseList = await _courseRepository.GetCourseListAsync(cancellationToken);
        if (courseList.Count == 0)
        {
            courseList.Add(new Course { CourseId = 0 });
        }
        _logger.LogInformation(message: "Returned from database and returned to the view {0} courses", courseList.Count);
        return courseList;
    }
}
