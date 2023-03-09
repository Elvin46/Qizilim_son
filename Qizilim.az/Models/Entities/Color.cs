using Qizilim.az.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Models.Entities
{
    public class Color : BaseEntity
    {
        public string Name { get; set; }

        public virtual ICollection<ProductColors> ProductColor { get; set; }



        public class ProductColors
        {

            public int ProductId { get; set; }
            public virtual Products Product { get; set; }
            public int ColorId { get; set; }
            public virtual Color Color { get; set; }
        }
    }
}
