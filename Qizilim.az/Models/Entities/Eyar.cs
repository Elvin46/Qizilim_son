using Qizilim.az.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Models.Entities
{
    public class Eyar : BaseEntity
    {
        public double EyarOlcusu { get; set; }

        public virtual ICollection<ProductEyar> ProductEyar { get; set; }
    }



    public class ProductEyar
    {

        public int ProductId { get; set; }
        public virtual Products Product { get; set; }
        public int EyarId { get; set; }
        public virtual Eyar Eyar { get; set; }
    }
}
