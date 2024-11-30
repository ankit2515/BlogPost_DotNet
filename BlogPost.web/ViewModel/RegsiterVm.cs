using Microsoft.AspNetCore.Mvc;
using Microsoft.Build.Framework;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages.Manage;
using System.ComponentModel.DataAnnotations;

namespace BlogPost.web.ViewModel
{
    public class RegsiterVm
    {
        [Microsoft.Build.Framework.Required]
        public string? FirstName { get; set; }
        [Microsoft.Build.Framework.Required]
        public string? LastName { get; set; }

        [Microsoft.Build.Framework.Required]
        public string? Email { get; set; }
        [Microsoft.Build.Framework.Required]
        public string? UserName { get; set; }
        [Microsoft.Build.Framework.Required]
        public string? Password { get; set; }
        public bool isAdmin{ get; set; }

    }
}
