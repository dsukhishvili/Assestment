﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using UserManagement.Service.DAL;

namespace UserManagement.Service.Models
{
    [Table("Phone")]
    public class Phone : IEntity
    {
        public int ID { get; set; }
        public PhoneTypes Type { get; set; }
        [StringLength(50, MinimumLength = 4)]
        public string Number { get; set; }
    }
}
