using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace UserManagement.Service.ValidationAttributes
{
    public class MinAgeAttribute : ValidationAttribute
    {
        private int _minAge { get; set; }
        public MinAgeAttribute(int minAge)
        {
            _minAge = minAge;
        }
        public override bool IsValid(object value)
        {
            return DateTime.Now.Year - ((DateTime)value).Year >= _minAge;
        }
    }
}
