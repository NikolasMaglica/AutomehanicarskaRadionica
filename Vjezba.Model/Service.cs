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
        [StringLength(40, ErrorMessage = "Naziv usluge može imati najviše 40 znakova.")]
        public string ServiceName { get; set; } = String.Empty;
        [Required(ErrorMessage = "Unos cijene usluge je obavezan.")]

        public decimal ServicePrice { get; set; }
        [StringLength(200, ErrorMessage = "Opis može imati najviše 200 znakova.")]
        public string ServiceDescription { get; set; } = String.Empty;
        public ICollection<ServiceOffer>? ServiceOffers { get; set; }

    }
}
