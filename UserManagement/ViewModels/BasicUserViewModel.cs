using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UserManagement.Service.Models;
using UserManagement.Service.ValidationAttributes;

namespace UserManagement.ViewModels
{
    public class BasicUserViewModel
    {
        [Required(ErrorMessage ="{0} is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage ="Text must be minimum length of 2 and max 50")]
        [RegularExpression("(^[a-zA-Z]+$|^[ა-ჰ]+$)",ErrorMessage ="Text must contain only latin or georgian symbols")]
        public string Firstname { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Text must be minimum length of 2 and max 50")]
        [RegularExpression("(^[a-zA-Z]+$|^[ა-ჰ]+$)", ErrorMessage = "Text must contain only latin or georgian symbols")]
        public string Lastname { get; set; }
        public Gender Gender { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [StringLength(11, MinimumLength = 11, ErrorMessage = "{0} must contain 11 symbols")]
        [RegularExpression("^[0-9]*$", ErrorMessage = "Text must contain only numbers")]
        public string IdentificationNumber { get; set; }
        [Required(ErrorMessage = "{0} is required")]
        [MinAge(18, ErrorMessage = "User must be at least 18 years old")]
        public DateTime DateOfBirth { get; set; }
        public int CityID { get; set; }
        public List<PhoneViewModel> Phones { get; set; }
    }
}
