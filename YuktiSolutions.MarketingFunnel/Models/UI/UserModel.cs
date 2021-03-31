using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace YuktiSolutions.MarketingFunnel.Models.UI
{
    public class UserModel
    {
        public Guid ID { get; set; }

        [DisplayName("Display Name")]
        [Required(ErrorMessage = "Display name cannot be left blank.")]
        [StringLength(256, ErrorMessage = "Display name cannot be more than {1} characters long.")]
        public String DisplayName { get; set; }

        [Required(ErrorMessage = "Email cannot be left blank.")]
        [StringLength(256, ErrorMessage = "Email cannot be more than {1} characters long.")]
        [EmailAddress(ErrorMessage = "Invalid email.Check the format.")]
        public String Email { get; set; }


        public Boolean IsPasswordChanged { get; set; }

        public string Password { get; set; }
       
        [DataType(DataType.Password)]
        [DisplayName("Compare Password ")]
        [Compare("Password", ErrorMessage = "Compare password do not match.")]
        public string ComparePassword { get; set; }

        [DisplayName("Phone Number")]
        //[Required(ErrorMessage = "Phone Number must be a Numeric")]
       [RegularExpression(@"^\(?([0-9]{3})\)?[-. ]?([0-9]{3})[-. ]?([0-9]{4})$", ErrorMessage = "Not a valid phone number")]
        public String PhoneNumber { get; set; }

        public Boolean IsActive { get; set; } 

        public String   Role { get; set; }
    }
}