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

        [ForeignKey(nameof(Client))]
        public int? ClientId { get; set; }
        public Client? Clients { get; set; }

        [ForeignKey(nameof(UserVehicle))]
        public int? UserVehicleId { get; set; }
        public UserVehicle? UserVehicles { get; set; }

        [ForeignKey(nameof(OfferStatus))]
        public int? OfferStatusId { get; set; }
        public OfferStatus? OfferStatuses { get; set; }

        public virtual List<MaterialOffer>? MaterialOffers { get; set; } = new List<MaterialOffer>();
        public virtual List<ServiceOffer>? ServiceOffers { get; set; } = new List<ServiceOffer>();
        [ForeignKey(nameof(AppUser))]
        public string? UsersId { get; set; }
        public AppUser? AppUser { get; set; }
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
