using Qizilim.az.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Models.Entities
{
    public class Shops : BaseEntity
    {
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public string Logo { get; set; }

        [ForeignKey("Centers")]
        public int CenterId { get; set; }
        public Centers Center { get; set; }

        //elaqe vasiteleri
        public string PhoneNumber { get; set; }
        public string WhatsappNumber { get; set; }
        public string InstagramAccount { get; set; }
        public string TiktokAccount { get; set; }


        
    }
}
