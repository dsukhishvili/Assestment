using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;
using UserManagement.Service.Models;

namespace UserManagement.Service.DTOModels
{
    public class PhoneDTO
    {
        public PhoneTypes Type { get; set; }
        public string Number { get; set; }
    }
}
