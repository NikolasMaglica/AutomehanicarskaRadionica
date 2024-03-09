
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Vjezba.Model
{
    public class Vehicle
    {
        public int ID { get; set; }
        [Required]
        public string ModelName { get; set; }
        public int ModelYear { get; set; }
        [ForeignKey(nameof(Manufacturer))]
        public int? ManufacturerID { get; set; }
        public Manufacturer? Manufacturer { get; set; }
        public virtual ICollection<UserVehicle>? UserVehicles { get; set; }

        public string? CreatedById { get; set; }
        public DateTime CreateTime { get; set; }

        public string? UpdatedById { get; set; }
        public DateTime UpdateTime { get; set; }
        public string? DeletedById { get; set; }
        public DateTime DeleteTime { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsDeleted { get; set; }= false;
    }
}
