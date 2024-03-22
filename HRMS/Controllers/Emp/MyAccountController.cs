﻿using HRMS.Data;
using HRMS.Models;
using HRMS.Models.Employees;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace HRMS.Controllers.Emp
{
    [Authorize]
    public class MyAccountController : Controller
    {

        private ApplicationDbContext _db;

        public MyAccountController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _db.Employees.Where(x => x.EmployeeUserID == userId).Include(x => x.Skills).ThenInclude(x => x.Skill).FirstOrDefault();
                if (user != null)
                {
                    return View(user);
                }
            }
            catch (Exception ex)
            {

            }

            return View();
        }
        public IActionResult Leaves()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Employees.Where(x => x.EmployeeUserID == userId).FirstOrDefault();
            if (user != null)
            {
                var leaves = _db.Leaves.Where(x => x.EmpId == user.Id).OrderBy(x => x.StartDate).ToList();
                return View(leaves);
            }


            return View();
        }

        public IActionResult ApplyLeave()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ApplyLeave([Bind("LeaveId,StartDate,EndDate,LeaveType,Reason,Status,EmpId,ApprovedBy")] Leave leave)
        {
            if (ModelState.IsValid)
            {
                leave.Status = 0;
                //var userId = HttpContext.Session.GetInt32("UserIdDB");
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                var user = _db.Employees.Where(x => x.EmployeeUserID == userId).FirstOrDefault();
                //if (userId != null && userId > 0)
                if (user != null)
                {
                    //leave.EmpId = (int)userId;
                    leave.EmpId = user.Id;
                    _db.Add(leave);
                    await _db.SaveChangesAsync();
                    return RedirectToAction(nameof(Leaves));
                }

            }
            return View(leave);
        }

        public async Task<IActionResult> Projects()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Employees.Where(x => x.EmployeeUserID == userId).FirstOrDefault();
            if (user != null)
            {
                var projects = _db.Projects.Where(x => x.AssignedEmployees.Any(y => y.Id == user.Id)).ToList();
                return View(projects);
            }
            return View();
        }

        public async Task<IActionResult> AssignSkill(int empId)
        {
            var empSkills = _db.EmpSkills.Where(x => x.EmpId == empId).Include(x => x.Skill).ToList();
            var totalSkills = _db.Skills.AsEnumerable().Where(x => !empSkills.Any(y => y.Skill.Id == x.Id)).ToList();
            AssignSkillModel model = new()
            {
                SkillTypes = totalSkills,
                EmpId = empId
            };
            return View(model);
        }
        public async Task<bool> AssignSkillToEmployee(int empId, int skillId)
        {
            try
            {
                var exist = _db.EmpSkills.Where(x => x.EmpId == empId && x.SkillId == skillId).FirstOrDefault();
                if (exist == null)
                {
                    EmpSkill obj = new()
                    {
                        EmpId = empId,
                        SkillId = skillId,
                        CreatedDate = DateTime.Now
                    };
                    _db.EmpSkills.Add(obj);
                    await _db.SaveChangesAsync();
                    return true;
                }

            }
            catch (Exception ex)
            {
                return false;
            }
            return false;
        }

        public async Task<bool> RemoveSkillForEmployee(int skillId)
        {
            var skill = _db.EmpSkills.Where(x => x.Id == skillId).FirstOrDefault();
            if (skill != null)
            {
                _db.EmpSkills.Remove(skill);
                await _db.SaveChangesAsync();
                return true;

            }

            return false;
        }
        public async Task<IActionResult> Tasks(int projId)
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var user = _db.Employees.Where(x => x.EmployeeUserID == userId).Include(x => x.Skills).ThenInclude(x => x.Skill).FirstOrDefault();
            if (user != null)
            {
                var tasks = _db.Tasks.Where(x => x.ProjectId == projId && x.AssignedToUserId == user.Id).IgnoreAutoIncludes()
                    .Include(x=>x.Comments).ThenInclude(x=>x.Attachements)
                    .Include(x=>x.Comments).ThenInclude(x=>x.User)
                    .Include(x => x.CreatedBy).Include(x => x.Project).ToList();
                return View(tasks);
            }
            return View();
        }

        //public async Task<ActionResult> Task(int TaskId)
        //{
        //    TaskSingle? task = await _db.Tasks.FirstOrDefaultAsync(x => x.TaskId == TaskId);
        //    if (task != null)
        //        return View(task);
        //    return NotFound();
        //}
        [HttpGet]
        public async Task<bool> ChangeTaskStatus(int taskId, int status)
        {
            var task = _db.Tasks.Where(x => x.TaskId == taskId).FirstOrDefault();
            if (task != null)
            {
                task.Status = status;
                task.CompletedDate = DateTime.Now;
                _db.Tasks.Update(task);
                await _db.SaveChangesAsync();
                return true;
            }
            return false;
        }


    }
    public class AssignSkillModel
    {
        public int EmpId { get; set; }
        public List<SkillType> SkillTypes { get; set; }
    }
}
