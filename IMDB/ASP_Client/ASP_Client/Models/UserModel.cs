using System;
using System.ComponentModel.DataAnnotations;

namespace ASP_Client.Models
{
    /// <summary>
    /// Class representing a model for login/registration
    /// </summary>
    [Serializable]
    public class UserModel
    {
        public string ErrorMsg { get; set; }

        [Required]
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