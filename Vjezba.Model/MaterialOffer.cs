using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class MaterialOffer
    {
        public int ID { get; set; }
        public int Quantity { get; set; }
        [ForeignKey(nameof(Offer))]
        public int? OfferId { get; set; }
        public Offer? Offers { get; set; }
        [ForeignKey(nameof(Material))]
        public int? MaterialId { get; set; }
        public Material? Materials { get; set; }
    }
}
