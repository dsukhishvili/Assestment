using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Service.Models;

namespace UserManagement.Service.DTOModels
{
    public class RelatedUserDto
    {
        public int RelatedUserId { get; set; }
        public RelationType RelationType { get; set; }
    }
}
