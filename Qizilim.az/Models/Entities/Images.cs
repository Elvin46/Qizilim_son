using Qizilim.az.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Models.Entities
{
    public class Images : BaseEntity
    {
        public string Path { get; set; }

        public virtual ICollection<ProductImage> ProductImages { get; set; }


        public class ProductImage
        {

            public int ProductId { get; set; }
            public virtual Products Product { get; set; }
            public int ImageId { get; set; }
            public virtual Images Image { get; set; }
        }
    }
}
