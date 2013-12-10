using System;
using System.ComponentModel.DataAnnotations;

namespace ASP_Client.Models
{
    [Serializable]
    public class UserModel
    {
        [Required]
        [EmailAddress]
        [StringLength(150)]
        [Display(Name = "Name: ")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }
    }
}