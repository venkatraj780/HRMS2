using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
//using HRMS.Migrations;
using HRMS.DTO;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.VisualBasic;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin, Superadmin")]
    public class EmployeesController : Controller
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ApplicationDbContext _context;
        private readonly RoleManager<IdentityRole> _roleManager;

        public EmployeesController(ApplicationDbContext context, UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager, IUserStore<IdentityUser> userStore, RoleManager<IdentityRole> roleManager)
        {
            _context = context;
            _userManager = userManager;
            _userStore = userStore;
            _signInManager = signInManager;
            _emailStore = GetEmailStore();
            _roleManager = roleManager;
        }

        // GET: Employees
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = await _context.Employees.Include(e => e.Department).Include(x => x.Address).Include(x => x.AssignedProjects).Include(x => x.Skills).ThenInclude(x => x.Skill).ToListAsync();
            return View(applicationDbContext);
        }

        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department).Include(x => x.Skills).ThenInclude(x => x.Skill).Include(x => x.Address)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(employee.EmployeeUserID);
            ViewData["Email"] = user?.Email;
            return View(employee);
        }

        // GET: Employees/Create
        public IActionResult Create()
        {

            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName");
            //List<IdentityRole> roles = _roleManager.Roles.Where(x => x.Name != "Superadmin").ToList();
            ViewData["Roles"] = new SelectList(_roleManager.Roles.Where(x => x.Name != "Superadmin").ToList(), "Name", "Name");
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("FirstName,LastName,Email,Password,DepartmentId,DateOfBirth,PhoneNumber,IsDepartmentManager,IsProjectManager,ApartmentNumber,HouseNumber,City,PostCode,Street,Role")] EmployeeRegisterDTO employeeDTO)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    var user = CreateUser();

                    await _userStore.SetUserNameAsync(user, employeeDTO.Email, CancellationToken.None);
                    await _emailStore.SetEmailAsync(user, employeeDTO.Email, CancellationToken.None);
                    var result = await _userManager.CreateAsync(user, employeeDTO.Password);

                    if (result.Succeeded)
                    {
                        Address address = new()
                        {
                            ApartmentNumber = employeeDTO.ApartmentNumber,
                            HouseNumber = employeeDTO.HouseNumber,
                            City = employeeDTO.City,
                            PostCode = employeeDTO.PostCode,
                            Street = employeeDTO.Street
                        };
                        Employee employee = new()
                        {
                            CreatedTime = DateTime.Now,
                            DateOfBirth = employeeDTO.DateOfBirth,
                            DepartmentId = employeeDTO.DepartmentId,
                            EmployeeUserID = user.Id,
                            PhoneNumber = employeeDTO.PhoneNumber,
                            FirstName = employeeDTO.FirstName,
                            LastName = employeeDTO.LastName,
                            IsActive = true,
                            IsDepartmentManager = false,
                            IsProjectManager = false,
                            Address = address
                        };
                        _context.Add(employee);
                        await _context.SaveChangesAsync();
                        var resultRoleAssigned = await _userManager.AddToRoleAsync(user, employeeDTO.Role);
                        if (resultRoleAssigned.Succeeded)
                            return RedirectToAction(nameof(Index));
                    }
                }
            }
            catch (Exception ex)
            {

                throw;
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", employeeDTO.DepartmentId);
            return View(employeeDTO);
            //if (ModelState.IsValid)
            //{
            //    _context.Add(employee);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "Id", employee.DepartmentId);
            //return View(employee);
        }
        // GET: Employees/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees.Where(x => x.Id == id).Include(x => x.Address).FirstOrDefaultAsync();
            if (employee == null)
            {
                return NotFound();
            }
            var user = await _userManager.FindByIdAsync(employee.EmployeeUserID);
            var model = new EmpEditDTO
            {
                Id = employee.Id,
                FirstName = employee.FirstName,
                LastName = employee.LastName,
                DateOfBirth = employee.DateOfBirth,
                DepartmentId = employee.DepartmentId,
                PhoneNumber = employee.PhoneNumber,
                IsActive = employee.IsActive,
                ApartmentNumber = employee.Address.ApartmentNumber,
                City = employee.Address.City,
                Email = user?.Email,
                HouseNumber = employee.Address.HouseNumber,
                PostCode = employee.Address.PostCode,
                Street = employee.Address.Street
            };
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", employee.DepartmentId);
            return View(model);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,DateOfBirth,DepartmentId,Email,IsActive,PhoneNumber,HouseNumber,ApartmentNumber,City,PostCode,Street")] EmpEditDTO employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existEmp = await _context.Employees.Where(x => x.Id == employee.Id).Include(x => x.Address).FirstOrDefaultAsync();
                    if (existEmp != null)
                    {
                        existEmp.FirstName = employee.FirstName;
                        existEmp.LastName = employee.LastName;
                        existEmp.PhoneNumber = employee.PhoneNumber;
                        existEmp.DateOfBirth = employee.DateOfBirth;
                        existEmp.DepartmentId = employee.DepartmentId;
                        existEmp.IsActive = employee.IsActive;
                        existEmp.Address.ApartmentNumber = employee.ApartmentNumber;
                        existEmp.Address.HouseNumber = employee.HouseNumber;
                        existEmp.Address.City = employee.City;
                        existEmp.Address.Street = employee.Street;
                        existEmp.Address.PostCode = employee.PostCode;
                        var user = await _userManager.FindByIdAsync(existEmp.EmployeeUserID);
                        if (user != null)
                        {
                            user.Email = employee.Email;
                            user.UserName = employee.Email;
                            var result = await _userManager.UpdateAsync(user);
                        }
                        _context.Employees.Update(existEmp);
                        await _context.SaveChangesAsync();
                    }
                    else
                    {
                        return NotFound();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DepartmentId"] = new SelectList(_context.Departments, "Id", "DepartmentName", employee.DepartmentId);
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .Include(e => e.Department).Include(x=>x.Address).Include(x=>x.Skills).ThenInclude(x=>x.Skill)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var employee = await _context.Employees.Where(x => x.Id == id).Include(x => x.Address).FirstOrDefaultAsync();
            if (employee != null)
            {
                var user = await _userManager.FindByIdAsync(employee.EmployeeUserID);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                    _context.Employees.Remove(employee);
                }
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }

        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            if (!_userManager.SupportsUserEmail)
            {
                throw new NotSupportedException("The default UI requires a user store with email support.");
            }
            return (IUserEmailStore<IdentityUser>)_userStore;
        }

        public async Task<IActionResult> AssignProject(int id)
        {
            ViewBag.Id = id;
            var empSkills = _context.EmpSkills.Where(x => x.EmpId == id).ToList();
            var projects = _context.Projects.Where(x => !x.AssignedEmployees.Any(y => y.Id == id)).Include(x => x.SkillsRequired).ToList();
            var proj = projects.Where(x => empSkills.Any(y => x.SkillsRequired.AsEnumerable().Any(z => z.SkillId == y.SkillId))).ToList();
            AssignProjectModel model = new() { Id = id, Projects = proj };
            return View(model);
        }
        public async Task<bool> AssignProjectToEmp(int id, int projId)
        {
            var proj = _context.Projects.Where(x => x.ProjectId == projId).FirstOrDefault();
            if (proj != null)
            {
                var emp = _context.Employees.Where(x => x.Id == id).FirstOrDefault();
                if (emp != null)
                {
                    emp.AssignedProjects.Add(proj);
                    await _context.SaveChangesAsync();
                    return true;
                }
            }
            return false;
        }
        public async Task<bool> UnAssignProjectToEmp(int id, int projId)
        {
            var emp = _context.Employees.Where(x => x.Id == id).Include(x => x.AssignedProjects).FirstOrDefault();
            if (emp != null)
            {
                var existProj = emp.AssignedProjects.Where(x => x.ProjectId == projId).FirstOrDefault();
                if (existProj != null)
                {
                    emp.AssignedProjects.Remove(existProj);
                    await _context.SaveChangesAsync();
                    return true;
                }

            }

            return false;
        }
    }
    public class AssignProjectModel
    {
        public int Id { get; set; }
        public List<Project> Projects { get; set; }
    }
}
