using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models
{
    public class WorkspaceMembership
    {
        public int WorkspaceMembershipID { get; set; }
        public int WorkspaceID { get; set; }
        public string ApplicationUserID { get; set; }
        public DateTime? JoinDate { get; set; }

        public Workspace Workspace { get; set; }
        public ApplicationUser ApplicationUser { get; set; }
    }
}
