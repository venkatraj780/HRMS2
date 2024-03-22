using HRMS.Data;
using HRMS.DTO;
using HRMS.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Net.Mail;
using System.Security.Claims;

namespace HRMS.Controllers
{
    public class CommentsController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;
        public CommentsController(UserManager<IdentityUser> userManager, ApplicationDbContext db)
        {
            _userManager = userManager;
            _db = db;
        }
        // GET: CommentsController
        public ActionResult Index()
        {
            return View();
        }

        // GET: CommentsController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        public ActionResult Create(int TaskId)
        {
            ViewBag.IsSaved = false;
            CommentCreateDTO obj = new CommentCreateDTO
            {
                Description = "",
                Attachements = new(),
                TaskId = TaskId
            };
            return View(obj);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(CommentCreateDTO comm)
        {
            try
            {
                List<CommentAttachement> AttachementsList = new();
                if (comm.Attachements.Count > 0)
                {
                    foreach (var file in comm.Attachements)
                    {
                        string path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Attachements");
                        if (!Directory.Exists(path))
                            Directory.CreateDirectory(path);

                        FileInfo fileInfo = new FileInfo(file.FileName);
                        string fileName = Guid.NewGuid() + fileInfo.Extension;
                        string fileNameWithPath = Path.Combine(path, fileName);
                        using var stream = new FileStream(fileNameWithPath, FileMode.Create);
                        file.CopyTo(stream);
                        AttachementsList.Add(new() { FileLocation = $"../../Attachements/{fileName}", FileName = file.FileName });
                    }
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = _db.Employees.Where(x => x.EmployeeUserID == userId).FirstOrDefault();
                    Comment commentNew = new()
                    {
                        Description = comm.Description,
                        CreatedAt = DateTime.Now,
                        TaskId = comm.TaskId,
                        UserId = user.Id,
                        Attachements = AttachementsList
                    };
                    _db.Comments.Add(commentNew);
                    await _db.SaveChangesAsync();
                    ViewBag.IsSaved = true;
                    return View();
                }
            }
            catch
            {
                return View();
            }
            return View();
        }


        // GET: CommentsController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: CommentsController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: CommentsController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: CommentsController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
