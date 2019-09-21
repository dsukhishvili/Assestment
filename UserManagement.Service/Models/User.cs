using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using UserManagement.Service.DAL;
using UserManagement.Service.ValidationAttributes;

namespace UserManagement.Service.Models
{
    [Table("User")]
    public class User:IEntity
    {
        public int ID { get; set; }
        [Required]
        [StringLength(50, MinimumLength =2)]
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
        public string PictureRelativePath { get; set; }
        public virtual ICollection<Phone> Phones { get; set; }
        public virtual ICollection<RelatedUser> ContactPersons { get; set; }
        public virtual City City { get; set; }
    }
}
