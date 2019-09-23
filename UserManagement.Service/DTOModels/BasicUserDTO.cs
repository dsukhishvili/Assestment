using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UserManagement.Service.Models;

namespace UserManagement.Service.DTOModels
{
    public class BasicUserDTO
    {
        public string Firstname { get; set; }
        public string Lastname { get; set; }
        public Gender Gender { get; set; }
        public string IdentificationNumber { get; set; }
        public DateTime DateOfBirth { get; set; }
        public int CityID { get; set; }
        public List<PhoneDTO> Phones { get; set; }
    }
}
