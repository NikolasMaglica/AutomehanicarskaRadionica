
using System.ComponentModel.DataAnnotations;


namespace Vjezba.Model
{
    public class Manufacturer:BaseEntity
    {
        public int ID { get; set; }
        [Required(ErrorMessage = "Unos imena proizvođača je obavezan.")]
        [StringLength(40, ErrorMessage = "Ime proizvođača ne može biti duže od 40 znakova.")]
        public string Name { get; set; }
        public virtual ICollection<Vehicle>? Vehicles { get; set; }
    }
}
