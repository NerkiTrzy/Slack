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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace Slack.Controllers
{
    public class WorkspacesController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IHostingEnvironment _environment;

        public WorkspacesController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, IHostingEnvironment environment)
        {
            _userManager = userManager;
            _context = context;
            _environment = environment;
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
                var channel = new Channel { Name = "general", OwnerID = _userManager.GetUserId(HttpContext.User), Type = Channel.ChannelType.Public, General = true, Workspace = workspace };
                _context.Add(workspace);
                _context.Add(channel);
                _context.Add(new WorkspaceMembership { WorkspaceID = workspace.ID, JoinDate = DateTime.Now, ApplicationUserID = workspace.OwnerID });
                _context.Add(new ChannelMembership { ChannelID = channel.ID, JoinDate = DateTime.Now, ApplicationUserID = workspace.OwnerID });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
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


        // GET: Workspaces/Messages/WorkspaceName/ChannelName
        public async Task<IActionResult> Messages(string id, string channel)
        {
            if (string.IsNullOrEmpty(id) || string.IsNullOrEmpty(channel))
            {
                return NotFound();
            }
            var workspace = await _context.Workspace.Include(ws => ws.WorkspaceMemberships).ThenInclude(wm => wm.ApplicationUser).Include(c => c.Channels).ThenInclude(chm => chm.ChannelMemberships)
                .SingleOrDefaultAsync(w => w.Name == id);
            var workspaceMembership = await _context.WorkspaceMembership
                .SingleOrDefaultAsync(m => m.WorkspaceID == workspace.ID && m.ApplicationUserID == _userManager.GetUserId(HttpContext.User));
            var currentChannel = await _context.Channel.Include(c => c.ChannelMemberships).ThenInclude(chm => chm.ApplicationUser).Include(c => c.Messages).ThenInclude(f => f.File)
                .SingleOrDefaultAsync(c => c.Name == channel && c.Workspace == workspace);
            var channelMembership = await _context.ChannelMembership.
                SingleOrDefaultAsync(chm => chm.ChannelID == currentChannel.ID && chm.ApplicationUserID == _userManager.GetUserId(HttpContext.User));
            if (workspace == null)
            {
                return NotFound();
            }
            if (workspaceMembership == null)
            {
                return RedirectToAction("WorkspaceAccessDenied", "Workspaces");
            }
            if (channelMembership == null)
            {
                return RedirectToAction("WorkspaceAccessDenied", "Workspaces");
            }
            if (currentChannel == null)
            {
                return NotFound();
            }

            return View(new WorkspaceViewModel
            {
                ID = workspace.ID,
                Name = workspace.Name,
                Channels = workspace.Channels,
                WorkspaceMemberships = workspace.WorkspaceMemberships,
                OwnerID = workspace.OwnerID,
                InviteUserViewModel = new InviteUserViewModel { WorkspaceName = id, InviterName = _userManager.GetUserName(HttpContext.User) },
                ChannelViewModel = new ChannelViewModel {Messages = currentChannel.Messages, Name = currentChannel.Name, General = currentChannel.General, ChannelMemberships = currentChannel.ChannelMemberships, ID = currentChannel.ID, OwnerID = currentChannel.OwnerID, Type = currentChannel.Type,
                ChannelInviteViewModel = new ChannelInviteViewModel { WorkspaceName = workspace.Name, ChannelName = channel }},
                CreateChannelViewModel = new CreateChannelViewModel { WorkspaceName = workspace.Name, OwnerID = _userManager.GetUserId(HttpContext.User) }
            });
        }

        public IActionResult WorkspaceAccessDenied()
        {
            return View();
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
                   // TempData["InvitationResultMessage"] = "That user is already a member of this workspace.";
                   // return RedirectToAction("Messages/" + model.WorkspaceName + "/general", "Workspaces");
                   return Json("That user is already a member of this workspace.");
                }
            }

            if (ModelState.IsValid)
            {

                Guid guid = Guid.NewGuid();

                var message = new MailMessage();
                message.To.Add(new MailAddress(model.EmailAddress)); 
                message.From = new MailAddress("199844bd68-7b4c7c@inbox.mailtrap.io");
                message.Subject = "SLACK Invitation";
                message.Body = $"<p>{model.InviterName} has invited you to join {model.WorkspaceName} on SLACK.</p>";
                message.Body += $"<a href=\"https://localhost:44320/WorkspaceInvitation?id={guid}\">https://localhost:44320/WorkspaceInvitation?id={guid}</a>";
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

                    //add to database
                    _context.Add(new WorkspaceInvitation { WorkspaceName = model.WorkspaceName, UserEmailAddress = model.EmailAddress,
                        InvitationGUID = guid, StartDate = DateTime.Now });
                    await _context.SaveChangesAsync();
                    //TempData["InvitationResultMessage"] = "The invitation has been sent.";

                }
            }
            //return RedirectToAction("Messages/" + model.WorkspaceName + "/general", "Workspaces");
            return Json("The invitation has been sent.");
        }

        [HttpPost, ActionName("UploadFile")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UploadFile(String id, String channel, IFormFile file)
        {
            id = id.Replace(" ", "_");
            channel = channel.Replace(" ", "_");

            var username = _userManager.GetUserName(HttpContext.User);
            var path = "uploads" + "/" + id + "/" + channel + "/" + username;
            var extension = System.IO.Path.GetExtension(file.FileName);
            var filename = Guid.NewGuid() + extension;
            var filepath = "/" + path + "/" + filename;

            var uploads = Path.Combine(_environment.WebRootPath, path);
            EnsureDirectory(uploads);

            if (file.Length > 0)
            {
                using (var fileStream = new FileStream(Path.Combine(uploads, filename), FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                   
                }
            }
            Models.File fileModel = new Models.File { FilePath = filepath, OriginalName = file.FileName, ContentType = file.ContentType };
            return Json(fileModel);
        }

        void EnsureDirectory(string path)
        {
            if ((path.Length > 0) && (!Directory.Exists(path)))
            {
                Directory.CreateDirectory(path);
            }
        }
    }
}
