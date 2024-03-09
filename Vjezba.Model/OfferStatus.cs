using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class OfferStatus
    {
        public int ID { get; set; }
        public string OfferStatusName { get; set; } = String.Empty;
        public virtual ICollection<Offer>? Offers { get; set; }

    }
}
