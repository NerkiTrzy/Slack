using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Slack.Data;
using Slack.Models;

namespace Slack.Controllers
{
    public class WorkspaceMembershipsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public WorkspaceMembershipsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: WorkspaceMemberships
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.WorkspaceMembership.Include(w => w.ApplicationUser).Include(w => w.Workspace);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: WorkspaceMemberships/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workspaceMembership = await _context.WorkspaceMembership
                .Include(w => w.ApplicationUser)
                .Include(w => w.Workspace)
                .SingleOrDefaultAsync(m => m.WorkspaceMembershipID == id);
            if (workspaceMembership == null)
            {
                return NotFound();
            }

            return View(workspaceMembership);
        }

        // GET: WorkspaceMemberships/Create
        public IActionResult Create()
        {
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id");
            ViewData["WorkspaceID"] = new SelectList(_context.Workspace, "ID", "ID");
            return View();
        }

        // POST: WorkspaceMemberships/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("WorkspaceMembershipID,WorkspaceID,ApplicationUserID,JoinDate")] WorkspaceMembership workspaceMembership)
        {
            if (ModelState.IsValid)
            {
                _context.Add(workspaceMembership);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", workspaceMembership.ApplicationUserID);
            ViewData["WorkspaceID"] = new SelectList(_context.Workspace, "ID", "ID", workspaceMembership.WorkspaceID);
            return View(workspaceMembership);
        }

        // GET: WorkspaceMemberships/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workspaceMembership = await _context.WorkspaceMembership.SingleOrDefaultAsync(m => m.WorkspaceMembershipID == id);
            if (workspaceMembership == null)
            {
                return NotFound();
            }
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", workspaceMembership.ApplicationUserID);
            ViewData["WorkspaceID"] = new SelectList(_context.Workspace, "ID", "ID", workspaceMembership.WorkspaceID);
            return View(workspaceMembership);
        }

        // POST: WorkspaceMemberships/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("WorkspaceMembershipID,WorkspaceID,ApplicationUserID,JoinDate")] WorkspaceMembership workspaceMembership)
        {
            if (id != workspaceMembership.WorkspaceMembershipID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workspaceMembership);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkspaceMembershipExists(workspaceMembership.WorkspaceMembershipID))
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
            ViewData["ApplicationUserID"] = new SelectList(_context.Users, "Id", "Id", workspaceMembership.ApplicationUserID);
            ViewData["WorkspaceID"] = new SelectList(_context.Workspace, "ID", "ID", workspaceMembership.WorkspaceID);
            return View(workspaceMembership);
        }

        // GET: WorkspaceMemberships/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workspaceMembership = await _context.WorkspaceMembership
                .Include(w => w.ApplicationUser)
                .Include(w => w.Workspace)
                .SingleOrDefaultAsync(m => m.WorkspaceMembershipID == id);
            if (workspaceMembership == null)
            {
                return NotFound();
            }

            return View(workspaceMembership);
        }

        // POST: WorkspaceMemberships/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workspaceMembership = await _context.WorkspaceMembership.SingleOrDefaultAsync(m => m.WorkspaceMembershipID == id);
            _context.WorkspaceMembership.Remove(workspaceMembership);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkspaceMembershipExists(int id)
        {
            return _context.WorkspaceMembership.Any(e => e.WorkspaceMembershipID == id);
        }
    }
}
