using DataAnnotationsExtensions;
using FeedseaWebSite.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FeedseaWebSite.Models
{
    public class FeedbackViewModel
    {
        [Display(Name = "Name", ResourceType=typeof(Strings))]
        [Required]
        public string Name { get; set; }

        [Display(Name = "Email", ResourceType=typeof(Strings))]
        [Required]
        [Email(ErrorMessageResourceName="InvalidEmail", ErrorMessageResourceType=typeof(Strings))]
        public string Email { get; set; }

        [Display(Name = "Description", ResourceType=typeof(Strings))]
        [Required]
        public string Description { get; set; }

        [Display(Name = "Type", ResourceType=typeof(Strings))]
        [Required]
        public string Type { get; set; }
    }
}