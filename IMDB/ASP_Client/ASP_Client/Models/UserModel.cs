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
      
        [Required]
        [StringLength(150, MinimumLength = 1)]
        [Display(Name = "Name: ")]
        public string Name { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [StringLength(20, MinimumLength = 6)]
        [Display(Name = "Password: ")]
        public string Password { get; set; }


        public string ErrorMsg { get; set; }

    }
}