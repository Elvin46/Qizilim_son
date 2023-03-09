using Qizilim.az.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Models.Entities
{
    public class Kateqoriya : BaseEntity
    {
        public string Name { get; set; }


        public virtual ICollection<ProductKateqoriya> ProductKateqoriya { get; set; }
    }



    public class ProductKateqoriya
    {

        public int ProductId { get; set; }
        public virtual Products Product { get; set; }
        public int KateqoriyaId { get; set; }
        public virtual Kateqoriya Kateqoriya { get; set; }
    }
}
