using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Manager.Models
{
    public class PartType
    {
        [Key]
        public int PartTypeId { get; set; }

        [Required]
        public string Section { get; set; }

        public List<Parts> parts { get; set; } = new List<Parts>();
    }
}
