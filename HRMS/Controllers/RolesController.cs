using HRMS.Data;
using HRMS.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;

namespace HRMS.Controllers
{
    [Authorize(Roles = "Admin, Superadmin")]
    public class RolesController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _context;
        public RolesController(RoleManager<IdentityRole> roleManager, ApplicationDbContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult Index()
        {
            List<IdentityRole> roles = _roleManager.Roles.ToList();
            return View(roles);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Name")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                var roleNew = new IdentityRole
                {
                    Name = role.Name
                };
                await _roleManager.CreateAsync(roleNew);
                return RedirectToAction(nameof(Index));
            }
            return View(role);
        }
        public async Task<IActionResult> Details(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles
                .FirstOrDefaultAsync(m => m.Id == id);
            if (role == null)
            {
                return NotFound();
            }

            return View(role);
        }

        public async Task<IActionResult> Edit(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }
            return View(role);
        }

        // POST: Departments/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name")] IdentityRole role)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    var roleExist = await _roleManager.FindByIdAsync(id);
                    if (roleExist != null)
                    {
                        roleExist.Name = role.Name;
                        await _roleManager.UpdateAsync(roleExist);
                        return RedirectToAction(nameof(Index));
                    }
                }
                catch (DbUpdateConcurrencyException)
                {

                }

            }
            return View(role);
        }

        // GET: Departments/Delete/5
        public async Task<IActionResult> Delete(string? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var roleExist = await _roleManager.FindByIdAsync(id);
            if (roleExist == null)
            {
                return NotFound();
            }

            return View(roleExist);
        }

        // POST: Departments/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var roleExist = await _roleManager.FindByIdAsync(id);
            if (roleExist != null)
            {
                var result = await _roleManager.DeleteAsync(roleExist);
            }

            return RedirectToAction(nameof(Index));
        }

    }
}
