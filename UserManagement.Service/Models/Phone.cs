using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UserManagement.Service.Models
{
    [Table("Phone")]
    public class Phone
    {
        public int ID { get; set; }
        public PhoneTypes Type { get; set; }
        [StringLength(50, MinimumLength = 4)]
        public string Number { get; set; }
    }
}
