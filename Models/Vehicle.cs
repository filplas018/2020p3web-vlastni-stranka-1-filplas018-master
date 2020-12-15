using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace P3_app_plass.Models
{
    public class Vehicle
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("GarageId")]
        public Garage Garage { get; set; }
        [Required]
        public int GarageId { get; set; }
        [Required]
        public string Brand { get; set;}
        [Required]
        public string Model { get; set; }
        public string Description { get; set; }
    }
}
