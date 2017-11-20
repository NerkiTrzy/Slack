using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models.WorkspaceViewModels
{
    public class WorkspaceViewModel
    {
        public int ID { get; set; }
        public string OwnerID { get; set; }
        public string Name { get; set; }
        public ICollection<WorkspaceMembership> WorkspaceMemberships { get; set; }
        public ICollection<Channel> Channels { get; set; }


        public ChannelViewModel ChannelViewModel { get; set; }
        public CreateChannelViewModel CreateChannelViewModel { get; set; }
        public InviteUserViewModel InviteUserViewModel { get; set; }
    }
}
