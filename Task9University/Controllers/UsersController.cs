using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Task9University.Controllers;
[Authorize(Roles ="superAdmin")]
public class UsersController : Controller
{
    private readonly UserManager<IdentityUser> _userManager;

    public UsersController(UserManager<IdentityUser> userManager)
    {
        _userManager = userManager;
    }

    public async Task<IActionResult> Index()
    {
        var users = await _userManager.Users.ToListAsync();
        var userRoleList = await _userManager.GetUsersInRoleAsync("user");
        var adminRoleList = await _userManager.GetUsersInRoleAsync("admin");
        
        Dictionary<string, List<IdentityUser>> keyValues = new Dictionary<string, List<IdentityUser>>();
        keyValues["user"] = (List<IdentityUser>)userRoleList;
        keyValues["admin"] = (List<IdentityUser>)adminRoleList;

        return View(keyValues);
    }
}
