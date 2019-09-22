using System;
using System.Collections.Generic;
using System.Text;
using UserManagement.Service.Models;

namespace UserManagement.Service.DTOModels
{
    public class RelatedPersonReportDTO
    {
        public int UserID { get; set; }
        public Dictionary<RelationType, int> Relations{ get; set; }
    }
}
