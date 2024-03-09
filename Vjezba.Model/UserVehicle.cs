using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
	public class UserVehicle
	{
		public int ID { get; set; }
		[Required]
		public int KilometersTraveled { get; set; }
		public string Description { get; set; }
		[ForeignKey(nameof(Vehicle))]
		public int? VehicleID { get; set; }
		public Vehicle? Vehicle { get; set; }
		[ForeignKey(nameof(AppUser))]
		public string? UserId { get; set; }
		public AppUser? AppUser { get; set; }
		public string? CreatedById { get; set; }
		public DateTime CreateTime { get; set; }

		public string? UpdatedById { get; set; }
		public DateTime UpdateTime { get; set; }
		public bool IsActive { get; set; } = true;
        public virtual ICollection<Offer>? Offers { get; set; }


    }
}
