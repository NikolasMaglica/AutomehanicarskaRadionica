using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class MaterialOffer
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Unos je obavezan")]

        [Range(1, 1000, ErrorMessage = "Unos mora biti između 1 i 1000")]
        public int Quantity { get; set; }
        [ForeignKey(nameof(Offer))]
        public int? OfferId { get; set; }
        public Offer? Offers { get; set; }
        [Required(ErrorMessage = "Unos materijala je obavezan")]

        [ForeignKey(nameof(Material))]
        public int? MaterialId { get; set; }
        public Material? Materials { get; set; }
    }
}
