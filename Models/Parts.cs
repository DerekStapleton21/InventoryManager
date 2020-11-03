using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Policy;
using System.Threading.Tasks;

namespace Inventory_Manager.Models
{
    public class Parts
    {
        [Key]
        public int PartsId { get; set; }

        public int PartTypeId { get; set; }

        public PartType partType { get; set; }

        [Required]
        public string PartName { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]

        public string Size { get; set; }

        [Required]
        public string SerialNumber { get; set; }

        public List<UserParts> UserParts { get; set; } = new List<UserParts>();

    }
}
