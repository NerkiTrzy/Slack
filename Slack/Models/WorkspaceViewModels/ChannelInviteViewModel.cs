using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models.WorkspaceViewModels
{
    public class ChannelInviteViewModel
    {
        [Required]
        [Display(Name = "User")]
        public string InvitedUserName { get; set; }
        public string ChannelName { get; set; }
        public string WorkspaceName { get; set; }
    }
}

