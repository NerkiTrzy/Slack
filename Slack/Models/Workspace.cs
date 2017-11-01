using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models
{
    public class Workspace
    {
        public int ID { get; set; }
        public string OwnerID { get; set; }
        public string Name { get; set; }
        public ICollection<WorkspaceMembership> WorkspaceMemberships { get; set; }
    }
}
