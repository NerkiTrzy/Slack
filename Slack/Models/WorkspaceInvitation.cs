using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models
{
    public class WorkspaceInvitation
    {
        public int WorkspaceInvitationID{ get; set; }
        public string WorkspaceName { get; set; }
        public string UserEmailAddress { get; set; }
        public Guid InvitationGUID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime? CloseDate { get; set; }
        public long? ExpirationTime { get; set; }

    }
}
