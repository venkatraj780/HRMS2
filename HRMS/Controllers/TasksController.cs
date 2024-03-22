using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis;
using Microsoft.EntityFrameworkCore;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin, Superadmin")]
    public class TasksController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;

        public TasksController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }

        public ActionResult List(int ProjId)
        {
            var proj=_db.Projects.Where(x=>x.ProjectId==ProjId).FirstOrDefault();
            if (proj != null)
            {
                ViewBag.ProjectName = proj.ProjectName;
            }
            else
            {
                ViewBag.ProjectName = "";
            }
            List<TaskSingle> tasks = _db.Tasks.Where(x => x.ProjectId == ProjId).IgnoreAutoIncludes()
                .Include(x => x.Comments).ThenInclude(x => x.Attachements)
                    .Include(x => x.Comments).ThenInclude(x => x.User)
                    .Include(x => x.CreatedBy).Include(x=>x.AssignedTo).Include(x => x.Project).ToList();
            return View(tasks);
        }

        //public ActionResult Details(int id)
        //{
        //    return View();
        //}

        public ActionResult Create(int ProjId)
        {
            TaskSingle task = new TaskSingle() { ProjectId = ProjId };
            var proj = _db.Projects.Where(x => x.ProjectId == ProjId).Include(x => x.AssignedEmployees).FirstOrDefault();

            ViewBag.AssignedToUserId = new SelectList(proj?.AssignedEmployees.Select(x => new SelectListItem
            {
                Value = x.Id.ToString(),
                Text = x.FirstName + " " + x.LastName
            }), "Value", "Text");
            return View(task);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create([Bind("Title,Description,Deadline,AssignedToUserId,ProjectId")] TaskSingle task)
        {
            try
            {
                var usr = await _userManager.GetUserAsync(HttpContext.User);
                if (usr != null)
                {
                    var CurrentUser = _db.Employees.Where(x => x.EmployeeUserID == usr.Id).FirstOrDefault();
                    if (CurrentUser != null)
                    {
                        TaskSingle taskNew = new()
                        {
                            Title = task.Title,
                            Description = task.Description,
                            Deadline = task.Deadline,
                            CreatedTime = DateTime.Now,
                            CreatedUserId = CurrentUser.Id,
                            AssignedToUserId = task.AssignedToUserId,
                            ProjectId = task.ProjectId,
                            Status = (int)CommentStatus.ToDo,
                            CompletedDate = task.Deadline.AddDays(1)
                        };
                        _db.Tasks.Add(taskNew);
                        await _db.SaveChangesAsync();
                        return RedirectToAction(nameof(Index), "Projects");
                    }
                }

                return View();
            }
            catch
            {
                return View();
            }
        }

        //public ActionResult Edit(int id)
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}

        //public ActionResult Delete(int id)
        //{
        //    return View();
        //}

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Delete(int id, IFormCollection collection)
        //{
        //    try
        //    {
        //        return RedirectToAction(nameof(Index));
        //    }
        //    catch
        //    {
        //        return View();
        //    }
        //}
    }
}
