using System.ComponentModel.DataAnnotations;

namespace Wp.Web.Api.Admin.Models.AccountViewModels
{
    public class LoginViewModel
    {
        [Required]
       /// [EmailAddress]
        public string Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
