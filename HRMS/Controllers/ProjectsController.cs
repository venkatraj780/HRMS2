using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using HRMS.Data;
using HRMS.Models;
using HRMS.Models.Employees;
using HRMS.Migrations;
using HRMS.Services;
using Microsoft.AspNetCore.Authorization;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin, Superadmin")]
    public class ProjectsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ProjectsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Projects
        public async Task<IActionResult> Index()
        {
            return View(await _context.Projects.Include(x => x.SkillsRequired).ThenInclude(x => x.Skill).IgnoreQueryFilters().ToListAsync());
        }

        // GET: Projects/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Include(x => x.SkillsRequired).ThenInclude(x => x.Skill).IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // GET: Projects/Create
        public IActionResult Create()
        {
            var skillTypes = _context.Skills.ToList();
            ViewBag.SkillTypes = skillTypes;
            return View();
        }

        // POST: Projects/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProjectId,ProjectName,StartDate,EndDate,CreatedTime,IsActive")] Project project, List<string> selSkills)
        {
            if (ModelState.IsValid)
            {
                project.CreatedTime = DateTime.Now;
                project.IsActive = true;
                _context.Add(project);
                await _context.SaveChangesAsync();
                if (selSkills != null && selSkills.Count > 0)
                {
                    foreach (var skillId in selSkills)
                    {
                        int.TryParse(skillId, out int skillIdInt);
                        var exist = _context.ProjSkills.Where(x => x.ProjId == project.ProjectId && x.SkillId == skillIdInt).FirstOrDefault();
                        if (exist == null)
                        {
                            ProjSkill obj = new()
                            {
                                ProjId = project.ProjectId,
                                SkillId = skillIdInt,
                                CreatedDate = DateTime.Now
                            };
                            _context.ProjSkills.Add(obj);
                            await _context.SaveChangesAsync();
                        }
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var skillTypes = _context.Skills.ToList();
            ViewBag.SkillTypes = skillTypes;
            return View(project);
        }

        // GET: Projects/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Where(x => x.ProjectId == id).Include(x => x.SkillsRequired).ThenInclude(x => x.Skill).IgnoreQueryFilters().FirstOrDefaultAsync();
            if (project == null)
            {
                return NotFound();
            }
            //var skillTypes = _context.Skills.AsEnumerable().Where(x => project.SkillsRequired.All(y => y.SkillId != x.Id)).ToList();
            var skillTypes = _context.Skills.ToList();
            ViewBag.SkillTypes = skillTypes;
            return View(project);
        }

        // POST: Projects/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProjectId,ProjectName,StartDate,EndDate,CreatedTime,IsActive")] Project project, List<string> selSkills)
        {
            if (id != project.ProjectId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(project);
                    await _context.SaveChangesAsync();
                    var existTotalSkills = _context.ProjSkills.Where(x => x.ProjId == project.ProjectId).ToList();
                    _context.ProjSkills.RemoveRange(existTotalSkills);
                    await _context.SaveChangesAsync();
                    if (selSkills != null && selSkills.Count > 0)
                    {
                        foreach (var skillId in selSkills)
                        {
                            int.TryParse(skillId, out int skillIdInt);
                            var exist = _context.ProjSkills.Where(x => x.ProjId == project.ProjectId && x.SkillId == skillIdInt).FirstOrDefault();
                            if (exist == null)
                            {
                                ProjSkill obj = new()
                                {
                                    ProjId = project.ProjectId,
                                    SkillId = skillIdInt,
                                    CreatedDate = DateTime.Now
                                };
                                _context.ProjSkills.Add(obj);
                                await _context.SaveChangesAsync();
                            }
                        }
                    }

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProjectExists(project.ProjectId))
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
            var skillTypes = _context.Skills.AsEnumerable().Where(x => project.SkillsRequired.All(y => y.SkillId != x.Id)).ToList();
            ViewBag.SkillTypes = skillTypes;
            return View(project);
        }

        // GET: Projects/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var project = await _context.Projects.Include(x => x.SkillsRequired).ThenInclude(x => x.Skill).IgnoreQueryFilters()
                .FirstOrDefaultAsync(m => m.ProjectId == id);
            if (project == null)
            {
                return NotFound();
            }

            return View(project);
        }

        // POST: Projects/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var project = await _context.Projects.IgnoreQueryFilters().Where(x => x.ProjectId == id).FirstOrDefaultAsync();
            if (project != null)
            {
                _context.Projects.Remove(project);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProjectExists(int id)
        {
            return _context.Projects.IgnoreQueryFilters().Any(e => e.ProjectId == id);
        }


    }
}
