using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Inventory_Manager.Models
{
    public class UserParts
    {
        [Key]
        public int UserPartsId { get; set; }

        [Required]

        public string UserId { get; set; }

        public ApplicationUser User { get; set; }

        [Required]

        public int PartsId { get; set; }

        public Parts Part { get; set; }
    }
}
