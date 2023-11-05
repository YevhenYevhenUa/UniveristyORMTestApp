using Microsoft.AspNetCore.Mvc;
using Task9.University.Services.Interfaces;
using Task9.University.Infrastructure.Presentations;
using Task9University.Models;
using Microsoft.AspNetCore.Authorization;

namespace Task9University.Controllers;

[Authorize]
public class StudentsController : Controller
{

    private readonly IStudentService _studentServices;

    public StudentsController(IStudentService studentServices)
    {
        _studentServices = studentServices;
    }

    public async Task<IActionResult> Index(int id, CancellationToken cancellationToken)
    {
        var studentList = await _studentServices.Index(id, cancellationToken);
        if (studentList is null)
        {
            return View("Error", new ErrorViewModel { RequestId = id.ToString() });
        }

        return View(studentList);
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var student = await _studentServices.EditStudentView(id, cancellationToken);
        if (student is null)
        {
            return View("Error", new ErrorViewModel { RequestId = id.ToString() });
        }

        return View(student);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditStudentViewModel studentVM, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit student");
            return View("Edit", studentVM);
        }

        var student = await _studentServices.EditStudent(studentVM, cancellationToken);
        if (student)
        {
            return RedirectToRoute("Default", new { Controller = "Students", Action = "Index", id = studentVM.GroupId });
        }
        else
        {
            return View(studentVM);
        }
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var student = await _studentServices.DeleteStudentView(id, cancellationToken);
        if (student is null)
        {
            return View("Error", new ErrorViewModel { RequestId = id.ToString() });
        }

        return View(student);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeleteStudentViewModel studentVM, CancellationToken cancellationToken)
    {
        var student = await _studentServices.DeleteStudent(studentVM, cancellationToken);
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to delete student");
            return View("Delete", student);
        }

        if (!student)
        {
            return View("Error", new ErrorViewModel { RequestId = studentVM.Id.ToString() });
        }

        return RedirectToRoute("Default", new { Controller = "Students", Action = "Index", id = studentVM.GroupId });

    }

    public async Task<IActionResult> Create(int id, CancellationToken cancellationToken)
    {
        var studentVM = await _studentServices.CreateStudentView(id, cancellationToken);
        if (studentVM is null)
        {
            return View("Error", new ErrorViewModel { RequestId = id.ToString() });
        }

        return View(studentVM);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateStudentViewModel studentVM, CancellationToken cancellationToken)
    {
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to delete student");
            return View(studentVM);
        }

        var student = await _studentServices.CreateNewStudent(studentVM, cancellationToken);
        if (!student)
        {
            return View("Error");
        }

        return RedirectToRoute("Default", new { Controller = "Students", Action = "Index", id = studentVM.GroupId });
    }

}
