using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Identity;
using Slack.Data;
using Slack.Models;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

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

        public async Task Send(string message, string author, string channel, Models.File file)
        {
            var currentChannel = await _context.Channel.SingleOrDefaultAsync(c => c.ID == Int32.Parse(channel));
            var users = from u in _context.Users
                        select u;
            var user = (ApplicationUser)users.Where(u => u.UserName.Equals(author)).First();
            var msg = new Message { ApplicationUser = user, ApplicationUserID = user.Id, Channel = currentChannel, ChannelID = currentChannel.ID, MessageText = message, SendDate = DateTime.Now, File = file };
            _context.Add(msg);
            await _context.SaveChangesAsync();
            await Clients.Group(channel).InvokeAsync("Send", $"{DateTime.Now.ToString()}: {author}: {message}");
        }

        public async Task SendFile(string message, string author, string channel, Models.File file)
        {
            var currentChannel = await _context.Channel.SingleOrDefaultAsync(c => c.ID == Int32.Parse(channel));
            var users = from u in _context.Users
                        select u;
            var user = (ApplicationUser)users.Where(u => u.UserName.Equals(author)).First();
            var msg = new Message { ApplicationUser = user, ApplicationUserID = user.Id, Channel = currentChannel, ChannelID = currentChannel.ID, MessageText = message, SendDate = DateTime.Now, File = file };
            _context.Add(msg);
            await _context.SaveChangesAsync();
            var json = SerializeFile(msg.File);
            await Clients.Group(channel).InvokeAsync("SendFile", $"{json}");
        }

        public async Task JoinGroup(string groupName)
        {
            await Groups.AddAsync(Context.ConnectionId, groupName);
        }

        public static string SerializeFile(Models.File file)
        {
            //Create a stream to serialize the object to.  
            MemoryStream ms = new MemoryStream();

            // Serializer the User object to the stream.  
            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(Models.File));
            ser.WriteObject(ms, file);
            byte[] json = ms.ToArray();
            ms.Close();
            return Encoding.UTF8.GetString(json, 0, json.Length);

        }
    }
}