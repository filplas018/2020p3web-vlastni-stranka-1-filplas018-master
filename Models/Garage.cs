using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace P3_app_plass.Models
{
    public class Garage
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        [ForeignKey("UserId")]
        public IdentityUser User { get; set; }
        [Required]
        public string UserId { get; set; }
        public ICollection<Vehicle> Vehicles { get; set; }
    }
}
