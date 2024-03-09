using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class BaseEntity
    {
        public string? CreatedById { get; set; }
        public DateTime CreateTime { get; set; }

        public string? UpdatedById { get; set; }
        public DateTime UpdateTime { get; set; }

        public string? DeletedById { get; set; }
        public DateTime DeleteTime { get; set; }

        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; } = false;
    }
}
