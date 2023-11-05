using Microsoft.AspNetCore.Mvc;
using Task9.University.Services.Interfaces;
using Task9.University.Infrastructure.Presentations;
using Task9University.Models;
using Microsoft.AspNetCore.Authorization;

namespace Task9University.Controllers;

[Authorize]
public class GroupsController : Controller
{
    private readonly IGroupService _groupService;

    public GroupsController(IGroupService groupService)
    {
        _groupService = groupService;
    }

    public async Task<IActionResult> Index(int id, CancellationToken cancellationToken)
    {
        var group = await _groupService.Index(id, cancellationToken);
        if (group is null)
        {
            return View("Error", new ErrorViewModel { RequestId = id.ToString() });
        }

        return View(group);
    }

    public async Task<IActionResult> Edit(int id, CancellationToken cancellationToken)
    {
        var groupVM = await _groupService.EditGroupView(id, cancellationToken);
        if(groupVM is null)
        {
            return View("Error", new ErrorViewModel { RequestId = id.ToString() });
        }

        return View(groupVM);
    }

    [HttpPost]
    public async Task<IActionResult> Edit(EditGroupViewModel groupVM, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to edit group");
            return View("Edit", groupVM);
        }

        var group = await _groupService.GroupEdit(groupVM, cancellationToken);
        if(!group)
        {
            return View(groupVM);
        }

        return RedirectToRoute("Default", new { Controller = "Groups", Action = "Index", id = groupVM.CourseId });
    }

    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        var group = await _groupService.DeleteGroupView(id, cancellationToken);
        if(group is null)
        {
            return View("Error", new ErrorViewModel { RequestId = id.ToString() });
        }

        return View(group);
    }

    [HttpPost]
    public async Task<IActionResult> Delete(DeleteGroupViewModel groupVM, CancellationToken cancellationToken)
    {
        var group = await _groupService.GroupDelete(groupVM, cancellationToken);
        if (!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Failed to delete group");
            return View("Delete", groupVM);
        }

        if (!group)
        {
            TempData["Error"] = "You can't delete a group if it doesn't have zero students";
            return View(groupVM);
        }

        return RedirectToRoute("Default", new { Controller = "Groups", Action = "Index", id = groupVM.CourseId });
    }

    public async Task<IActionResult> Create(int id, CancellationToken cancellationToken)
    {
        var groupVM = await _groupService.CreateGroupView(id, cancellationToken);
        if (groupVM is null)
        {
            return View("Error", new ErrorViewModel { RequestId = id.ToString() });
        }

        return View(groupVM);
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateGroupViewModel groupVM, CancellationToken cancellationToken)
    {
        if(!ModelState.IsValid)
        {
            ModelState.AddModelError("", "Something went wrong while creating group");
            return View(groupVM);
        }

        var group = await _groupService.CreateGroup(groupVM, cancellationToken);
        if(!group)
        {
            return View(groupVM);
        }

        return RedirectToRoute("Default", new { Controller = "Groups", Action = "Index", id = groupVM.CourseId });
    }

}
