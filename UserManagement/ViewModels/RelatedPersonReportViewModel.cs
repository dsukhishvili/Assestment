using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Service.Models;

namespace UserManagement.ViewModels
{
    public class RelatedPersonReportViewModel
    {
        public int UserID { get; set; }
        public Dictionary<RelationType, int> Relations{ get; set; }
    }
}
