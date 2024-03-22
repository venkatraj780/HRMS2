using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Services
{
    public class SuperadminService
    {
        public static async Task SuperadminRole(IServiceProvider serviceProvider)
        {
            using (var scope = serviceProvider.CreateScope())
            {
                //Resolve ASP .NET Core Identity with DI help
                var userManager = (UserManager<IdentityUser>)scope.ServiceProvider.GetService(typeof(UserManager<IdentityUser>));
                var roleManager = (RoleManager<IdentityRole>)scope.ServiceProvider.GetService(typeof(RoleManager<IdentityRole>));
                var _db = (ApplicationDbContext)scope.ServiceProvider.GetService(typeof(ApplicationDbContext));
                // do you things here

                //var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
                //var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
                bool isSuperadminRoleExist = await roleManager.RoleExistsAsync("Superadmin");

                if (!isSuperadminRoleExist)
                {
                    var role = new IdentityRole();
                    role.Name = "Superadmin";
                    await roleManager.CreateAsync(role);
                }
                var isSuperAdmin = await userManager.FindByEmailAsync("superadmin@gmail.com");
                if (isSuperAdmin == null)
                {
                    var user = new IdentityUser();
                    user.UserName = "superadmin@gmail.com";
                    user.Email = "superadmin@gmail.com";
                    user.EmailConfirmed = true;

                    string SuperadminPassword = "Superadmin123!@#";

                    IdentityResult identityResult = await userManager.CreateAsync(user, SuperadminPassword);

                    if (identityResult.Succeeded)
                    {
                        var result = await userManager.AddToRoleAsync(user, "Superadmin");
                    }
                    Department dept = new() { CreatedTime = DateTime.Now,
                    DepartmentName="HR",
                    IsActive=true};
                    _db.Departments.Add(dept);
                    await _db.SaveChangesAsync();
                    Address address = new()
                    {
                        ApartmentNumber = "100",
                        HouseNumber = "100",
                        City = "City",
                        PostCode = "PostCode",
                        Street = "Street"
                    };
                    Employee employee = new()
                    {
                        CreatedTime = DateTime.Now,
                        DateOfBirth = DateTime.Now,
                        DepartmentId = dept.Id,
                        EmployeeUserID = user.Id,
                        PhoneNumber = "0123456789",
                        FirstName = "Super",
                        LastName = "Admin",
                        IsActive = true,
                        IsDepartmentManager = false,
                        IsProjectManager = false,
                        Address = address
                    };
                    _db.Add(employee);
                    await _db.SaveChangesAsync();

                }
            }
        }
    }
}
