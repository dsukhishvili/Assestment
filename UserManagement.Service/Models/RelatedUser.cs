using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UserManagement.Service.Models
{
    [Table("RelatedUser")]
    public class RelatedUser
    {
        public int ID { get; set; }
        public RelationType RelationType { get; set; }
        public int RelatedUserID { get; set; }
        public virtual User User { get; set; }
    }
}
