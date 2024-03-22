using HRMS.Data;
using HRMS.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin, Superadmin")]
    public class AdminController : Controller
    {
        private ApplicationDbContext _db;

        public AdminController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Dashboard()
        {
            ViewData["Employees"] = _db.Employees.Count();
            ViewData["Projects"] = _db.Projects.Count();
            ViewData["Depts"] = _db.Departments.Count();
            ViewData["Leaves"] = _db.Leaves.Where(x => x.StartDate > DateTime.Now).Count();
            var projects = _db.Projects.Select(x => new ProjEmpCountModel { ProjectName = x.ProjectName, EmpCount = x.AssignedEmployees.Count() }).ToList();
            return View(projects);
        }

        public IActionResult EmpProjChart()
        {
            var projects = _db.Projects.Select(x => new ProjEmpCountModel { ProjectName = x.ProjectName, EmpCount = x.AssignedEmployees.Count() }).ToList();
            return View(projects);
        }

    }
}
