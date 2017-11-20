using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Slack.Data;
using Microsoft.AspNetCore.Identity;
using Slack.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace Slack.Controllers
{
    [AllowAnonymous]
    public class WorkspaceInvitationController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public WorkspaceInvitationController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task<IActionResult> Index(Guid ID)
        {

            //Check if the GUID is correct
            var invitations = from i in _context.WorkspaceInvitation
                        select i;
            var invitationExists = invitations.Any(i => i.InvitationGUID.Equals(ID));
            if(!invitationExists)
            {
                return NotFound();
            }


            //Check if the invitation has been already accepted/rejected
            var invitation = (WorkspaceInvitation)invitations.Where(i => i.InvitationGUID.Equals(ID)).First();

            if(invitation.CloseDate != null)
            {
                return RedirectToAction("Error", "WorkspaceInvitation", new { errorCode = 1});
            }


            //Check if the user has a Slack account
            var users = from u in _context.Users
                          select u;
            var userExists = users.Any(e => e.Email.Equals(invitation.UserEmailAddress));
            if(!userExists)
            {
                return RedirectToAction("Register", "Account", new { returnUrl =$"/WorkspaceInvitation?id={invitation.InvitationGUID}" , email=invitation.UserEmailAddress });
            }


            //Check if the user is already a member of the workspace
            List<WorkspaceMembership> allMemberships = await _context.WorkspaceMembership.Include(w => w.Workspace).Include(u => u.ApplicationUser).ToListAsync();
            foreach (WorkspaceMembership wm in allMemberships)
            {
                if (wm.Workspace.Name.Equals(invitation.WorkspaceName) && wm.ApplicationUser.Email.Equals(invitation.UserEmailAddress))
                {
                    return RedirectToAction("Error", "WorkspaceInvitation", new { errorCode = 2 });
                }
            }


            return View(invitation);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Accept(Guid guid)
        {
            var invitations = from i in _context.WorkspaceInvitation
                              select i;
            var invitationExists = invitations.Any(i => i.InvitationGUID.Equals(guid));
            if (!invitationExists)
            {
                return NotFound();
            }
            var invitation = (WorkspaceInvitation)invitations.Where(i => i.InvitationGUID.Equals(guid)).First();

            var users = from u in _context.Users
                              select u;
            var user = (ApplicationUser)users.Where(u => u.Email.Equals(invitation.UserEmailAddress)).First();
            var workspaces = from w in _context.Workspace
                        select w;
            var workspace = (Workspace)workspaces.Where(w => w.Name.Equals(invitation.WorkspaceName)).First();

            invitation.CloseDate = DateTime.Now;
            _context.Update(invitation);
            _context.Add(new WorkspaceMembership { WorkspaceID = workspace.ID, JoinDate = DateTime.Now, ApplicationUserID = user.Id});
           
            //add user to general channel
            var generalChannel = _context.Channel.Where(channel => channel.Workspace.ID == workspace.ID && channel.General == true).First();
            _context.Add(new ChannelMembership { ChannelID = generalChannel.ID, JoinDate = DateTime.Now, ApplicationUserID = user.Id });


            await _context.SaveChangesAsync();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Decline(Guid guid)
        {
            var invitations = from i in _context.WorkspaceInvitation
                              select i;
            var invitationExists = invitations.Any(i => i.InvitationGUID.Equals(guid));
            if (!invitationExists)
            {
                return NotFound();
            }

            var invitation = (WorkspaceInvitation)invitations.Where(i => i.InvitationGUID.Equals(guid)).First();
            invitation.CloseDate = DateTime.Now;
            _context.Update(invitation);
            await _context.SaveChangesAsync();
            return View();
        }

        public IActionResult Error(int errorCode)
        {
            return View(errorCode);
        }
    }
}