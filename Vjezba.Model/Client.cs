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

        [Required(ErrorMessage = "Unos imena je obavezan")]

        [MinLength(3, ErrorMessage = "Unesite barem 3 znaka")]
        public string FirstName { get; set; }
        [Required(ErrorMessage = "Unos prezimena je obavezan")]

        [MinLength(3, ErrorMessage = "Unesite barem 3 znaka")]
        public string LastName { get; set; }

        [Required(ErrorMessage = "Unos emaila je obavezan")]
        [EmailAddress(ErrorMessage = "Unesite ispravan e-mail format")]
        public string Email { get; set; }
        [Required(ErrorMessage = "Unos adrese je obavezan")]
        [StringLength(40)]
        public string? Address { get; set; }
        [Required(ErrorMessage = "Unos broja telefona je obavezan")]

        [Phone(ErrorMessage = "Unesite ispravan broj telefona")]
        public string? PhoneNumber { get; set; }

        public string FullName => $"{FirstName} {LastName}";
        public virtual ICollection<Offer>? Offers { get; set; }


    }
}
