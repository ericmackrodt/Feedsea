using DataAnnotationsExtensions;
using FeedseaWebSite.App_GlobalResources;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace FeedseaWebSite.Models
{
    public class JoinBetaViewModel
    {
        [Display(ResourceType=typeof(Strings), Name="Name")]
        [Required]
        public string Name { get; set; }

        [Display(ResourceType = typeof(Strings), Name = "Email")]
        [Required]
        [Email(ErrorMessageResourceName = "InvalidEmail", ErrorMessageResourceType = typeof(Strings))]
        public string Email { get; set; }
        [Display(ResourceType = typeof(Strings), Name = "WhereDidYouHear")]
        public string WhereDidYouHear { get; set; }
    }
}