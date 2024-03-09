using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Order
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Unos datuma je obavezan.")]
        public DateTime? Date { get; set; }
        public decimal TotalPrice { get; set; }
        [Required(ErrorMessage = "Unos statusa narudžbe je obavezan.")]

        [ForeignKey(nameof(OrderStatus))]
        public int? OrderStatusId { get; set; }
        public OrderStatus? OrderStatus { get; set; }
        public virtual List<OrderMaterial>? OrderMaterials { get; set; } = new List<OrderMaterial>();
		public string? CreatedById { get; set; }
		public DateTime CreateTime { get; set; }

		public string? UpdatedById { get; set; }
		public DateTime UpdateTime { get; set; }
		public string? DeletedById { get; set; }
		public DateTime DeleteTime { get; set; }
		public bool IsActive { get; set; } = true;
		public bool IsDeleted { get; set; } = false;

	}
}
