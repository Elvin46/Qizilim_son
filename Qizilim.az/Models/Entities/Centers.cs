using Qizilim.az.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Models.Entities
{
    public class Centers : BaseEntity
    {
        [Required]
        public string Name { get; set; }

        
        public List<Shops> Shop { get; set; }
    }
}
