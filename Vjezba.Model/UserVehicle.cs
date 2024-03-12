using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
	public class UserVehicle:BaseEntity
	{
		public int ID { get; set; }
        [Required(ErrorMessage = "Unos prijeđenih kilometara je obavezan.")]
        [Range(1, 999999, ErrorMessage = "Kilometraža može biti između 1 i 999 999 kilometara")]
		public int KilometersTraveled { get; set; }
        [StringLength(200)]
        public string? Description { get; set; }

        [ForeignKey(nameof(Vehicle))]
        [Required(ErrorMessage = "Unos vozila je obavezan.")]

        public int? VehicleID { get; set; }
		public Vehicle? Vehicle { get; set; }

        [ForeignKey(nameof(AppUser))]
        [Required(ErrorMessage = "Unos zaposlenika je obavezan.")]

        public string? UserId { get; set; }
		public AppUser? AppUser { get; set; }
		
        public virtual ICollection<Offer>? Offers { get; set; }


    }
}
