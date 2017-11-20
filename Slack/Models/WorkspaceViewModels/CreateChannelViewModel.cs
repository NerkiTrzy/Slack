using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models.WorkspaceViewModels
{
    public class CreateChannelViewModel
    {
        [Required]
        [Display(Name = "Name")]
        public string Name { get; set; }

        [Required]
        [EnumDataType(typeof(Channel.ChannelType))]
        public Channel.ChannelType Type { get; set; }

        public string OwnerID { get; set; }

        public string WorkspaceName { get; set; }
    }
}
