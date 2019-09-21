using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UserManagement.Service.Models;
using UserManagement.Service.ValidationAttributes;

namespace UserManagement.Service.DTOModels
{
    public class UserDTO
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression("(^[a-zA-Z]+$|^[ა-ჰ]+$)")]
        public string Firstname { get; set; }
        [Required]
        [StringLength(50, MinimumLength = 2)]
        [RegularExpression("(^[a-zA-Z]+$|^[ა-ჰ]+$)")]
        public string Lastname { get; set; }
        public Gender Gender { get; set; }
        [Required]
        [StringLength(11, MinimumLength = 11)]
        public string IdentificationNumber { get; set; }
        [Required]
        [MinAge(18)]
        public DateTime DateOfBirth { get; set; }
        public int CityID { get; set; }
        public string Picture { get; set; }
    }
}
