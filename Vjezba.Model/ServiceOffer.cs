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
        public int Quantity { get; set; }
        [ForeignKey(nameof(Service))]
        public int? ServiceId { get; set; }
        public Service? Services { get; set; }
        [ForeignKey(nameof(Offer))]
        public int? OfferId { get; set; }
        public Offer? Offers { get; set; }

    }
}
