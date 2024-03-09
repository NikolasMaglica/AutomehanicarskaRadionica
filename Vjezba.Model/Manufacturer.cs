using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Manufacturer:BaseEntity
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Unos imena proizvođača je obavezan.")]
        public string Name { get; set; }
        public virtual ICollection<Vehicle>? Vehicles { get; set; }

    }
}
