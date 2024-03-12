using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Service:BaseEntity
    {

        public int ID { get; set; }
        [Required(ErrorMessage = "Unos naziva usluge je obavezan.")]
        [StringLength(40)]
        public string ServiceName { get; set; } = String.Empty;
        [Required(ErrorMessage = "Unos cijene usluge je obavezan.")]
        [RegularExpression(@"^\d{1,9}(\.\d{1,2})?$", ErrorMessage = "Cijena mora imati najviše 9 mjesta prije i 2 mjesta poslije decimalne točke.")]
        public decimal ServicePrice { get; set; }
        [StringLength(200)]
        public string? ServiceDescription { get; set; }
        public ICollection<ServiceOffer>? ServiceOffers { get; set; }

    }
}
