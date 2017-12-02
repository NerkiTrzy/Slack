using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models
{
    public class Message
    {
        public int MessageID { get; set; }
        public int ChannelID { get; set; }
        public string ApplicationUserID { get; set; }
        public string MessageText { get; set; }
        public DateTime SendDate { get; set; }
        public File File { get; set; }

        public Channel Channel { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
