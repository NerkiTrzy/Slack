using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Slack.Data;
using Slack.Models;
using Slack.Models.WorkspaceViewModels;

namespace Slack.Controllers
{
    public class ChannelsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public ChannelsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        [HttpPost, ActionName("CreateChannel")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateChannel(CreateChannelViewModel model)
        {
            //check if the channel name is already taken
            var workspace = await _context.Workspace
                .Where(ws => ws.Name == model.WorkspaceName).SingleOrDefaultAsync();
            List<Channel> workspaceChannels = await _context.Channel
                .Where(ch => ch.Workspace.ID == workspace.ID).ToListAsync();
            foreach (Channel ch in workspaceChannels)
            {
                if(ch.Name.Equals(model.Name))
                {
                    return Json(new { result = "That name is already taken." });
                }
            }

            //create channel, add current user to it and redirect to the new channel
            var channel = new Channel { Name = model.Name, General = false, OwnerID = model.OwnerID, Type = model.Type, Workspace = workspace};
            _context.Add(channel);
            var channelMembership = new ChannelMembership { JoinDate = DateTime.Now, Channel = channel, ChannelID = channel.ID, ApplicationUserID = model.OwnerID };
            _context.Add(channelMembership);
            await _context.SaveChangesAsync();
            return Json(new { result = "Redirect", url = Url.Action("Messages/" + model.WorkspaceName + "/" + channel.Name, "Workspaces") });
        }

        public async Task<IActionResult> InviteUser(ChannelInviteViewModel model)
        {
            //check if the user is already a member of the channel
            var channel = await _context.Channel.
                Where(ch => ch.Name == model.ChannelName && ch.Workspace.Name == model.WorkspaceName).SingleOrDefaultAsync();
            var channelMembership = await _context.ChannelMembership.
                Where(chm => chm.ChannelID == channel.ID && chm.ApplicationUser.UserName == model.InvitedUserName).SingleOrDefaultAsync();
            if(channelMembership != null)
            {
                return Json(new { result = "That user is already a member of this channel." });
            }

            //check if the user is a member of the workspace
            var workspaceMembership = await _context.WorkspaceMembership.
                Where(wm => wm.Workspace.Name == model.WorkspaceName && wm.ApplicationUser.UserName == model.InvitedUserName).SingleOrDefaultAsync();
            if (workspaceMembership == null)
            {
                return Json(new { result = "That user isn't a member of this workspace." });
            }

            //add invited user to the channel
            var users = from u in _context.Users
                        select u;
            var user = (ApplicationUser)users.Where(u => u.UserName.Equals(model.InvitedUserName)).First();
            channelMembership = new ChannelMembership { JoinDate = DateTime.Now, Channel = channel, ChannelID = channel.ID, ApplicationUserID = user.Id};
            _context.Add(channelMembership);
            await _context.SaveChangesAsync();
            return Json(new { result = "Redirect", url = Url.Action("Messages/" + model.WorkspaceName + "/" + channel.Name, "Workspaces") });
        }


     /*   public async Task<IActionResult> GetAllMessages(string channel)
        {
            return Json("testmsg");
        }
        */
    }


}