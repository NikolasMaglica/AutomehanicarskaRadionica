using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class Client:BaseEntity
    {
        [Key]
        public int ID { get; set; }

        [Required]
        [MinLength(3, ErrorMessage = "Unesite barem 3 znaka")]
        public string FirstName { get; set; } = "";

        [Required]
        public string LastName { get; set; } = "";

        [Required]
        public string Email { get; set; } = "";

        public string? Address { get; set; }
        public string? PhoneNumber { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public virtual ICollection<Offer>? Offers { get; set; }


    }
}
