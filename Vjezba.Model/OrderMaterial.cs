using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class OrderMaterial
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Unos količine materijala je obavezan.")]
        [Range(1, int.MaxValue, ErrorMessage = "Količina mora biti veća od 0.")]
        public int Quantity { get; set; }

        [ForeignKey(nameof(Order))]
        public int? OrderId { get; set; }
        public Order? Order { get; set; }
        [Required(ErrorMessage = "Unos materijala je obavezan.")]

        [ForeignKey(nameof(Material))]
        public int? MaterialId { get; set; }
        public Material? Material { get; set; }

    }
}
