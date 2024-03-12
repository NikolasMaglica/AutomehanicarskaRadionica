using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class ServiceOffer
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Unos količine je obavezan.")]
        [Range(1, 1000, ErrorMessage = "Unos mora biti između 1 i 1000")]
        public int Quantity { get; set; }
        [Required(ErrorMessage = "Unos usluuge je obavezan.")]

        [ForeignKey(nameof(Service))]
        public int? ServiceId { get; set; }
        public Service? Services { get; set; }
        [ForeignKey(nameof(Offer))]
        public int? OfferId { get; set; }
        public Offer? Offers { get; set; }

    }
}
