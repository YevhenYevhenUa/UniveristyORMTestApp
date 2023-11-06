using Microsoft.AspNetCore.Identity;
using System.Net;
using Task9.University.Domain.Core;
using Task9.University.Infrastructure.Data;

namespace Task9University;

public static class UniversityInitializer
{
    public static WebApplication Seed(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();

        context.Database.EnsureCreated();

        var courses = context.Courses.FirstOrDefault();
        if (courses is null)
        {
            context.Courses.AddRange(
                new Course { Name = "Scala Course", Description = "Something usefull!" },
                new Course { Name = "C# course", Description = "from zero to hero" },
                new Course { Name = "Frontend course", Description = "You will learn how to paint buttons properly" },
                new Course { Name = "Backend coure", Description = "You will learn how to do everything in this life" }
            );

            context.Groups.AddRange(
                new Group { CourseId = 1, Name = "Beginners" },
                new Group { CourseId = 2, Name = "VM-61-1" },
                new Group { CourseId = 3, Name = "Dummies" },
                new Group { CourseId = 4, Name = "Gigachads" }
            );

            context.Students.AddRange(
                new Student { FirstName = "SomeOne", LastName = "FromNowhere", GroupId = 1 },
                new Student { FirstName = "TestName", LastName = "TestSureName", GroupId = 2 },
                new Student { FirstName = "Teodosdr", LastName = "asdrqw", GroupId = 3 },
                new Student { FirstName = "dsasdqwe", LastName = "cxxxds", GroupId = 1 },
                new Student { FirstName = "dfwqe", LastName = "ghgvb", GroupId = 2 },
                new Student { FirstName = "dfcx", LastName = "qwrrs", GroupId = 4 },
                new Student { FirstName = "fgghnnwr", LastName = "asdfgf", GroupId = 1 },
                new Student { FirstName = "werwerwe", LastName = "dfqwe", GroupId = 4 },
                new Student { FirstName = "asd", LastName = "asd", GroupId = 3 }
            );

            context.SaveChanges();
        }

        return app;
    }

    public static async Task<WebApplication> SeedUsersAndRoles(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();
        using var context = scope.ServiceProvider.GetRequiredService<AppDbContext>();
        using var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
        using var userManager = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

        if (!await roleManager.RoleExistsAsync(UserRoles.SuperAdmin))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.SuperAdmin));
        if (!await roleManager.RoleExistsAsync(UserRoles.Admin))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.Admin));
        if (!await roleManager.RoleExistsAsync(UserRoles.User))
            await roleManager.CreateAsync(new IdentityRole(UserRoles.User));


        string superAdminEmail = "superadminemail@gmail.com";
        string password = "uqywu@ioehui12gh4!";

        var appSuperAdmin = await userManager.FindByEmailAsync(superAdminEmail);

        if (appSuperAdmin == null)
        {
            var newAdminUser = new User()
            {
                Email = superAdminEmail,
                UserName = superAdminEmail,
            };

            await userManager.CreateAsync(newAdminUser, password);
            await userManager.AddToRoleAsync(newAdminUser, UserRoles.SuperAdmin);
        }

        string appAdminEmail = "adminemail@etickets.com";

        var appAdmin = await userManager.FindByEmailAsync(appAdminEmail);
        if (appAdmin == null)
        {
            var newAppUser = new User()
            {
                Email = appAdminEmail,
                UserName = appAdminEmail,
            };

            await userManager.CreateAsync(newAppUser, password);
            await userManager.AddToRoleAsync(newAppUser, UserRoles.Admin);
        }

        string appUserEmail = "useremail@etickets.com";

        var appUser = await userManager.FindByEmailAsync(appUserEmail);
        if (appUser == null)
        {
            var newAppUser = new User()
            {
                Email = appUserEmail,
                UserName = appUserEmail
            };

            await userManager.CreateAsync(newAppUser, password);
            await userManager.AddToRoleAsync(newAppUser, UserRoles.User);
        }

        return app;
    }
}
