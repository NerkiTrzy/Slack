using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Slack.Data;
using Slack.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace Slack.Hubs
{
    public class Chat : Hub
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public Chat(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
            _context = context;
        }

        public async Task Send(string message, string author, string channel)
        {
            var currentChannel = await _context.Channel.SingleOrDefaultAsync(c => c.ID == Int32.Parse(channel));
            var users = from u in _context.Users
                        select u;
            var user = (ApplicationUser)users.Where(u => u.UserName.Equals(author)).First();
            var msg = new Message { ApplicationUser = user, ApplicationUserID = user.Id, Channel = currentChannel, ChannelID = currentChannel.ID, MessageText = message, SendDate = DateTime.Now };
            _context.Add(msg);
            await _context.SaveChangesAsync();
            await Clients.Group(channel).InvokeAsync("Send", $"{DateTime.Now.ToString()}: {author}: {message}");
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddAsync(Context.ConnectionId, groupName);
        }
    }
}