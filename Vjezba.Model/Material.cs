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
        public string MaterialName { get; set; }
        [Required(ErrorMessage = "Unos količine na lageru je obavezan.")]
        [Range(1, 1000, ErrorMessage = "Unos mora biti između 1 i 1000")]

        public int InStockQuantity { get; set; }
        [Required(ErrorMessage = "Unos cijene je obavezan.")]
        [RegularExpression(@"^\d{1,9}(\.\d{1,2})?$", ErrorMessage = "Cijena mora imati najviše 9 mjesta prije i 2 mjesta poslije decimalne točke.")]
        public decimal MaterialPrice { get; set; }
        [StringLength(200)]
        public string? MaterialDescription { get; set; }
        public virtual ICollection<OrderMaterial>? OrderMaterials { get; set; }
        public virtual ICollection<MaterialOffer>? MaterialOffers { get; set; }




    }
}
