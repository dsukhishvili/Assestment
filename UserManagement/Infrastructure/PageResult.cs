using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UserManagement.Infrastructure
{
    public class PageResult<T>
    {
        public long? TotalCount { get; set; }
        public T Items { get; set; }
    }
}