using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models
{
    public class ChannelMembership
    {
        public int ChannelMembershipID { get; set; }
        public int ChannelID { get; set; }
        public string ApplicationUserID { get; set; }
        public DateTime? JoinDate { get; set; }


        public Channel Channel { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
