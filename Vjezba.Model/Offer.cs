using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Offer:BaseEntity
    {
        public int ID { get; set; }
        public decimal TotalPrice { get; set; }

        [Required(ErrorMessage = "Unos klijenta je obavezan")]

        [ForeignKey(nameof(Client))]
        public int? ClientId { get; set; }
        public Client? Clients { get; set; }
        [Required(ErrorMessage = "Unos  vozila je obavezan")]

        [ForeignKey(nameof(UserVehicle))]
        public int? UserVehicleId { get; set; }
        public UserVehicle? UserVehicles { get; set; }
        [Required(ErrorMessage = "Unos statusa ponude je obavezan")]

        [ForeignKey(nameof(OfferStatus))]
        public int? OfferStatusId { get; set; }
        public OfferStatus? OfferStatuses { get; set; }

        public virtual List<MaterialOffer>? MaterialOffers { get; set; } = new List<MaterialOffer>();
        public virtual List<ServiceOffer>? ServiceOffers { get; set; } = new List<ServiceOffer>();
        [Required(ErrorMessage = "Unos zaposlenika je obavezan")]

        [ForeignKey(nameof(AppUser))]
        public string? UsersId { get; set; }
        public AppUser? AppUser { get; set; }

    }
}
