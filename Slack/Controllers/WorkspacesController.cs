using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Slack.Data;
using Slack.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Slack.Models.WorkspaceViewModels;
using System.Net.Mail;
using System.Net;

namespace Slack.Controllers
{
    public class WorkspacesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkspacesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        // GET: Workspaces
        public async Task<IActionResult> Index()
        {

            var userId =  _userManager.GetUserId(HttpContext.User);
            List<WorkspaceMembership> allMemberships = await _context.WorkspaceMembership.ToListAsync();
            List<Workspace> allWorkspaces = await _context.Workspace.ToListAsync();
            List<Workspace> userWorkspaces = new List<Workspace>();
            foreach (WorkspaceMembership um in allMemberships)
            {
                if(um.ApplicationUserID.Equals(userId))
                {
                    userWorkspaces.Add(allWorkspaces.Find(a => a.ID == um.WorkspaceID));
                }
            }

            return View(userWorkspaces);
        }

        // GET: Workspaces/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workspace = await _context.Workspace
                .SingleOrDefaultAsync(m => m.ID == id);
            if (workspace == null)
            {
                return NotFound();
            }

            return View(workspace);
        }

        // GET: Workspaces/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Workspaces/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateWorkspaceViewModel model)
        {


            bool nameTaken;
            var workspaces = from w in _context.Workspace
                             select w;
            if (!String.IsNullOrEmpty(model.Name))
            {
                nameTaken = workspaces.Any(w => w.Name == model.Name);
                if(nameTaken)
                {
                    ModelState.AddModelError("Name", "That name is already taken.");
                }
            }

            if (ModelState.IsValid)
            {
                var workspace = new Workspace { Name = model.Name, OwnerID = _userManager.GetUserId(HttpContext.User) };
                _context.Add(workspace);
                _context.Add(new WorkspaceMembership { WorkspaceID = workspace.ID, JoinDate = DateTime.Now, ApplicationUserID = workspace.OwnerID });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: Workspaces/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workspace = await _context.Workspace.SingleOrDefaultAsync(m => m.ID == id);
            if (workspace == null)
            {
                return NotFound();
            }
            return View(workspace);
        }

        // POST: Workspaces/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ID,OwnerID,Name")] Workspace workspace)
        {
            if (id != workspace.ID)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(workspace);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkspaceExists(workspace.ID))
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
            return View(workspace);
        }

        // GET: Workspaces/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var workspace = await _context.Workspace
                .SingleOrDefaultAsync(m => m.ID == id);
            if (workspace == null)
            {
                return NotFound();
            }

            return View(workspace);
        }

        // POST: Workspaces/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var workspace = await _context.Workspace.SingleOrDefaultAsync(m => m.ID == id);
            _context.Workspace.Remove(workspace);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkspaceExists(int id)
        {
            return _context.Workspace.Any(e => e.ID == id);
        }


        // GET: Workspaces/Messages/WorkspaceName
        public async Task<IActionResult> Messages(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var workspace = await _context.Workspace.Include(ws => ws.WorkspaceMemberships).ThenInclude(wm => wm.ApplicationUser)
                .SingleOrDefaultAsync(w => w.Name == id);
            var workspaceMembership = await _context.WorkspaceMembership
                .SingleOrDefaultAsync(m => m.WorkspaceID == workspace.ID && m.ApplicationUserID == _userManager.GetUserId(HttpContext.User));

            if (workspace == null)
            {
                return NotFound();
            }
            if (workspaceMembership == null)
            {
                return RedirectToAction("WorkspaceAccessDenied", "Workspaces");
            }

            return View(workspace);
        }

        public IActionResult WorkspaceAccessDenied()
        {
            return View();
        }

        // GET: Workspaces/InviteUser/WorkspaceName
        public async Task<IActionResult> InviteUser(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }
            var workspace = await _context.Workspace.Include(ws => ws.WorkspaceMemberships).ThenInclude(wm => wm.ApplicationUser)
                .SingleOrDefaultAsync(w => w.Name == id);
            var workspaceMembership = await _context.WorkspaceMembership
                .SingleOrDefaultAsync(m => m.WorkspaceID == workspace.ID && m.ApplicationUserID == _userManager.GetUserId(HttpContext.User));

            if (workspace == null)
            {
                return NotFound();
            }
            if (workspaceMembership == null)
            {
                return RedirectToAction("WorkspaceAccessDenied", "Workspaces");
            }

            InviteUserViewModel model = new InviteUserViewModel { WorkspaceName = id, InviterName = _userManager.GetUserName(HttpContext.User) };
            return View(model);
        }

        // POST: Workspaces/InviteUser/WorkspaceName
        [HttpPost, ActionName("InviteUser")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> InviteUser(InviteUserViewModel model)
        {
            var workspaces = from w in _context.Workspace
                             select w;
            List<WorkspaceMembership> allMemberships = await _context.WorkspaceMembership.Include(w => w.Workspace).Include(u => u.ApplicationUser).ToListAsync();
            foreach (WorkspaceMembership wm in allMemberships)
            {
                if (wm.Workspace.Name.Equals(model.WorkspaceName) && wm.ApplicationUser.Email.Equals(model.EmailAddress))
                {
                    ModelState.AddModelError("EmailAddress", "That user is already a member of this workspace.");
                }
            }

            if (ModelState.IsValid)
            {
                //@TODO
                //add invitation link and create entry in database

                var message = new MailMessage();
                message.To.Add(new MailAddress(model.EmailAddress)); 
                message.From = new MailAddress("199844bd68-7b4c7c@inbox.mailtrap.io");
                message.Subject = "SLACK Invitation";
                message.Body = $"{model.InviterName} has invited you to join {model.WorkspaceName} on SLACK.";
                message.IsBodyHtml = true;

                using (var smtp = new SmtpClient())
                {
                    var credential = new NetworkCredential
                    {
                        UserName = "411d49481d15a7",
                        Password = "a57d38c47b205a"
                    };
                    smtp.Credentials = credential;
                    smtp.Host = "smtp.mailtrap.io";
                    smtp.Port = 25;
                    smtp.EnableSsl = true;
                    await smtp.SendMailAsync(message);
                    return RedirectToAction("Messages/" + model.WorkspaceName, "Workspaces");
                }
            }
            return View(model);
        }
    }
}
