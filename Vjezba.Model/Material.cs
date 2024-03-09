using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Material:BaseEntity
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Unos naziva je obavezan.")]
        [StringLength(40)]
        public string MaterialName { get; set; } = String.Empty;
        [Required(ErrorMessage = "Unos količine na lageru je obavezan.")]

        public int InStockQuantity { get; set; }
        [Required(ErrorMessage = "Unos cijene je obavezan.")]
        public decimal MaterialPrice { get; set; }
        [StringLength(200)]

        public string MaterialDescription { get; set; } = String.Empty;
        public virtual ICollection<OrderMaterial>? OrderMaterials { get; set; }
        public virtual ICollection<MaterialOffer>? MaterialOffers { get; set; }




    }
}
