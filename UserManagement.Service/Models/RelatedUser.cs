using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using UserManagement.Service.DAL;

namespace UserManagement.Service.Models
{
    [Table("RelatedUser")]
    public class RelatedUser : IEntity
    {
        public int ID { get; set; }
        public int UserID { get; set; }
        public RelationType RelationType { get; set; }
        public int RelatedUserID { get; set; }
    }
}
