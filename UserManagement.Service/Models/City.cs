using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace UserManagement.Service.Models
{
    [Table("City")]
    public class City
    {
        public int ID { get; set; }
        public string Name { get; set; }
    }
}
