using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Task9.University.Services.Interfaces;
using Task9.University.Infrastructure.Presentations;
using Task9University.Models;
using Task9.University.Domain.Core;
using Microsoft.AspNetCore.Authorization;
using Task9University.Paging;

namespace Task9University.Controllers;

[Authorize]
public class HomeController : Controller
{

    private readonly ICourseService _courseService;

    public HomeController(ICourseService courseService)
    {
        _courseService = courseService;
    }

    public async Task<IActionResult> Index(int? pageNumber, CancellationToken cancellationToken)
    {
        int pageSize = 5;
        var courseList = await _courseService.GetAllCourses(cancellationToken);
        var paginatedList = PaginatedList<Course>.Create(courseList.ToList(), pageNumber ?? 1, pageSize);
        return View(paginatedList);
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var CourseVM = await _courseService.EditCourseView(id, cancellationToken);

        if (CourseVM is null)
        {
            return View("Error", new ErrorViewModel { RequestId = id.ToString() });
        }

        return View(CourseVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditCourseViewModel courseVM, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit course");
            return View("Edit", courseVM);
        }

        var course = await _courseService.EditCourse(courseVM, cancellationToken);
        if (!course)
        {
            return View(courseVM);
        }

        return RedirectToAction("Index", "Home");
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var courseVM = await _courseService.DeleteCourseView(id, cancellationToken);

        if (courseVM is null)
        {
            return View("Error", new ErrorViewModel { RequestId = id.ToString() });
        }

        return View(courseVM);

    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeleteCourseViewModel courseVM, CancellationToken cancellationToken)
    {

        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to delete course");
            return View("Delete", courseVM);
        }

        var course = await _courseService.DeleteCourse(courseVM, cancellationToken);

        if (!course)
        {
            TempData["Error"] = "You can't delete a course if it doesn't have zero groups";
            return View(courseVM);
        }

        return RedirectToAction("Index", "Home");

    }
    public async Task<IActionResult> Create()
    {
        return View("Create");
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateCourseViewModel courseVM, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Something went wrong while group");
            return View(courseVM);
        }
        var student = await _courseService.CreateNewCourse(courseVM, cancellationToken);

        if (!student)
        {
            return View("Error");
        }

        return RedirectToAction("Index", "Home");

    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}