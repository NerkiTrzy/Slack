using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models.WorkspaceViewModels
{
    public class ChannelViewModel
    {
        public int ID { get; set; }
        public string OwnerID { get; set; }
        public string Name { get; set; }
        public bool General { get; set; }
        public Channel.ChannelType Type { get; set; }
        public Workspace Workspace { get; set; }
        public ICollection<ChannelMembership> ChannelMemberships { get; set; }
        public ICollection<Message> Messages { get; set; }

        public ChannelInviteViewModel ChannelInviteViewModel { get; set; }
    }
}
