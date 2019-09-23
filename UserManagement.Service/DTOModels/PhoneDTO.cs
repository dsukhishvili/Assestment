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
        [StringLength(50, MinimumLength = 4, ErrorMessage ="{0} must be minimum length of 4 and maximum 50")]
        public string Number { get; set; }
    }
}
