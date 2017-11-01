using Slack.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Slack.Models.WorkspaceViewModels
{
    public class CreateWorkspaceViewModel
    {

        [Required]
        [Display(Name = "Name")]
        // [NotUsed]
        public string Name { get; set; }



        /*public class NotUsed : ValidationAttribute
        {

            protected override ValidationResult IsValid(object value, ValidationContext validationContext)
            {
                CreateWorkspaceViewModel model = (CreateWorkspaceViewModel)validationContext.ObjectInstance;
                if ()
                    return new ValidationResult("That name is already taken.");

                return ValidationResult.Success;
            }
        }
    }*/
    }
}
