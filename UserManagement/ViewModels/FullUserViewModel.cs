using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Service.Models;
using UserManagement.ViewModels;

namespace UserManagement.ViewModels
{
    public class FullUserViewModel
    {
        public int ID { get; set; }
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Gender Gender { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CityID { get; set; }
        public List<PhoneViewModel> Phones { get; set; }
        public List<RelatedUserViewModel> RelatedUsers { get; set; }
        public string RelativePath { get; set; }
        public string ImageBase64 { get; set; }
    }
}
