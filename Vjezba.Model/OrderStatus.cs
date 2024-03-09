using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class OrderStatus
    {
        public int ID { get;set; }
        public string Name { get;set; }
        public virtual ICollection<Order>? Orders { get; set; }

    }
}
