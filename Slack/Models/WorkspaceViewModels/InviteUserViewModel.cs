using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models.WorkspaceViewModels
{
    public class InviteUserViewModel
    {
        [Required]
        [Display(Name = "Email Adress")]
        [EmailAddress]
        public string EmailAddress { get; set; }

        public string InviterName { get; set; }

        public string WorkspaceName { get; set; }
    }
}
