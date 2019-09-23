using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Service.Models;

namespace UserManagement.ViewModels
{
    public class RelatedUserViewModel
    {
        public int RelatedUserId { get; set; }
        public RelationType RelationType { get; set; }
    }
}
