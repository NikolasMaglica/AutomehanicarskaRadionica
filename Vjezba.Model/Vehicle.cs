
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace Vjezba.Model
{
    public class Vehicle:BaseEntity
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Unos modela vozila je obavezan")]
        [StringLength(40, ErrorMessage = "Maksimalno možete unijeti 40 znakova.")]
        public string ModelName { get; set; }
        [Required(ErrorMessage = "Unos godine modela je obavezan")]
        [RegularExpression(@"^\d{4}$", ErrorMessage = "Unesite točno četiri znamenke.")]
        [Range(1970, 2024, ErrorMessage = "Unesite godinu između 1970 i trenutne godine.")]

        public int ModelYear { get; set; }

        [ForeignKey(nameof(Manufacturer))]
        [Required(ErrorMessage = "Unos proizvođača vozila je obavezan")]

        public int? ManufacturerID { get; set; }
        public Manufacturer? Manufacturer { get; set; }
        public virtual ICollection<UserVehicle>? UserVehicles { get; set; }
    }
}
