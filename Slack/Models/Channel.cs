using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models
{
    public class Channel
    {
        public int ID { get; set; }
        public string OwnerID { get; set; }
        public string Name { get; set; }
        public bool General { get; set; }
        public ChannelType Type { get; set; }
        public Workspace Workspace { get; set; }
        public ICollection<ChannelMembership> ChannelMemberships { get; set; }


        public enum ChannelType { Private, Public };
    }
}
