using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vjezba.Model
{
    public class AppUser : IdentityUser
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public string Surname { get; set; }
        public virtual ICollection<UserVehicle>? UserVehicles { get; set; }
        public virtual ICollection<Offer>? Offers { get; set; }


    }
}
