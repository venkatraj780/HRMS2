using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Authorization;
using HRMS.Services;
using Microsoft.AspNetCore.Identity;
using HRMS.Helpers;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin, Superadmin")]
    public class LeavesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<IdentityUser> _userManager;
        public LeavesController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        // GET: Leaves
        public async Task<IActionResult> Index()
        {
            var leaves = await _context.Leaves.Include(x => x.EmployeeApplied).Include(x => x.EmployeeApproved)
                .Include(x => x.SickLeaveReplacedEmployee).ThenInclude(x => x.EmployeeOnleave).Include(x => x.SickLeaveReplacedEmployee)
                .ThenInclude(x => x.EmployeeReplaced).OrderByDescending(x => x.LeaveId).ToListAsync();
            return View(leaves);
        }

        // GET: Leaves/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves
                .FirstOrDefaultAsync(m => m.LeaveId == id);
            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }

        // GET: Leaves/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Leaves/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("LeaveId,StartDate,EndDate,LeaveType,Reason,Status,EmpId,ApprovedBy")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                _context.Add(leave);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(leave);
        }

        // GET: Leaves/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves.FindAsync(id);
            if (leave == null)
            {
                return NotFound();
            }
            return View(leave);
        }

        // POST: Leaves/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("LeaveId,StartDate,EndDate,Reason,Status,EmpId,ApprovedBy")] Leave leave)
        {
            if (id != leave.LeaveId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(leave);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LeaveExists(leave.LeaveId))
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
            return View(leave);
        }

        // GET: Leaves/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves
                .FirstOrDefaultAsync(m => m.LeaveId == id);
            if (leave == null)
            {
                return NotFound();
            }

            return View(leave);
        }

        // POST: Leaves/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var leave = await _context.Leaves.FindAsync(id);
            if (leave != null)
            {
                _context.Leaves.Remove(leave);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LeaveExists(int id)
        {
            return _context.Leaves.Any(e => e.LeaveId == id);
        }

        public async Task<IActionResult> Approve(int id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var leave = await _context.Leaves
                .FirstOrDefaultAsync(m => m.LeaveId == id);
            if (leave == null)
            {
                return NotFound();
            }
            leave.Status = 1;
            var userId = HttpContext.Session.GetInt32("UserIdDB");
            if (userId != null && userId > 0)
            {
                leave.ApprovedBy = userId;
            }
            else
            {
                return NotFound();
            }
            await _context.SaveChangesAsync();

            if (leave.LeaveType == 0)
            {
                Task.Run(async () => await SendEmailsToOtherEmployees(leave.EmpId, leave)).Wait();
            }
            return RedirectToAction(nameof(Index));
        }
        public async Task<IActionResult> Reject(int id)
        {
            var leave = await _context.Leaves
                .FirstOrDefaultAsync(m => m.LeaveId == id);
            if (leave == null)
            {
                return NotFound();
            }
            leave.Status = 2;
            var userId = HttpContext.Session.GetInt32("UserIdDB");
            if (userId != null && userId > 0)
            {
                leave.ApprovedBy = userId;
            }
            else
            {
                return NotFound();
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AssignEmployee(int leaveId, int empId)
        {
            empId = 1;
            var empCurr = _context.Employees.Where(x => x.Id == empId).Include(x => x.Skills).ThenInclude(x => x.Skill).FirstOrDefault();
            var empSkills = empCurr.Skills.Select(x => x.Skill).ToList();
            List<Employee>? employees = _context.Employees.Where(x => x.Id != empId && (x.DepartmentId == empCurr.DepartmentId || x.Skills.Any(y => empSkills.Contains(y.Skill))))
                .Include(x => x.Department).Include(x => x.Skills).ThenInclude(x => x.Skill).Include(x => x.AssignedProjects).OrderBy(x => x.AssignedProjects.Count).ToList();
            var ord = GetWorkload(employees);
            AssignEmployeeModel model = new()
            {
                Employees = ord,
                EmpId = empId,
                LeaveId = leaveId
            };
            return View(model);
        }
        private List<EmpWorkload> GetWorkload(List<Employee> empList)
        {
            //Dictionary<Employee,double> myDict = new();
            List<EmpWorkload> list = new();
            foreach (var emp in empList)
            {
                var tasks = _context.Tasks.Where(x => x.AssignedToUserId == emp.Id).ToList();
                WorkloadMetrics wm = new(tasks.Count(x => x.Status == 3), tasks.Count(x => x.Status == 2), tasks.Count(x => x.Status == 1));
                EmpWorkload empWorkload = new(emp, wm.WorkloadIndex);
                //empWorkload.Workload = wm.WorkloadIndex;
                list.Add(empWorkload);
                //myDict.Add(emp, wm.WorkloadIndex);
            }
            //var ordered = myDict.OrderBy(x => x.Value);
            //var sortedDict = from entry in myDict orderby entry.Value ascending select entry.Key;
            //return sortedDict.ToList();
            return list.OrderBy(x => x.Workload).ToList();
        }
        public async Task<bool> AssignEmployeeToSickLeave(int leaveId, int empId, int empIdReplaced)
        {
            try
            {
                var exist = _context.SickLeaveReplacedEmployees.Where(x => x.LeaveId == leaveId).FirstOrDefault();
                if (exist == null)
                {
                    SickLeaveReplacedEmployee obj = new()
                    {
                        LeaveId = leaveId,
                        EmpIdOnLeave = empId,
                        EmpReplcedId = empIdReplaced
                    };
                    _context.SickLeaveReplacedEmployees.Add(obj);
                    await _context.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }
        public async Task SendEmailsToOtherEmployees(int empId, Leave leave)
        {
            var empCurr = _context.Employees.Where(x => x.Id == empId).Include(x => x.Skills).ThenInclude(x => x.Skill).FirstOrDefault();
            var empSkills = empCurr.Skills.Select(x => x.Skill).ToList();
            var employees = _context.Employees.Where(x => x.Id != empId && (x.DepartmentId == empCurr.DepartmentId || x.Skills.Any(y => empSkills.Contains(y.Skill)))).ToList();
            foreach (Employee emp in employees)
            {
                var user = await _userManager.FindByIdAsync(emp.EmployeeUserID);
                if (user != null)
                {
                    string toEmail = user.Email;
                    string Subject = @"Sick Leave Replacement";
                    string Body = $"Hi {emp.FirstName} {emp.LastName},\n   You can work in the place of {empCurr.FirstName} {empCurr.LastName} as he is on sick leave from {leave.StartDate} to {leave.EndDate}.";
                    Email.Send(toEmail, Subject, Body);
                }
            }
        }
    }
    public class AssignEmployeeModel
    {
        public int LeaveId { get; set; }
        public int EmpId { get; set; }
        public List<EmpWorkload> Employees { get; set; }
    }
    public class EmpWorkload : Employee
    {
        public EmpWorkload(Employee emp, double workload)
        {
            Workload = workload;
            this.Id = emp.Id;
            this.FirstName = emp.FirstName;
            this.LastName = emp.LastName;
            this.Department = emp.Department;
            this.Skills = emp.Skills;
            this.AssignedProjects = emp.AssignedProjects;
        }
        public double? Workload { get; set; }
    }
}
