using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations;

namespace BlogPost.web.ViewModel
{
    public class ResetPasswordVm
    {
        public string? Id{ get; set; }
        public string? Username{ get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        public string? newPassword{ get; set; }

        [Compare(nameof(newPassword))]
        [System.ComponentModel.DataAnnotations.Required]
        public string? confirmPassword {  get; set; }

    }
}
