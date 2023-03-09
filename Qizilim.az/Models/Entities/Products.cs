using Qizilim.az.AppCode.InfraStructure;
using Qizilim.az.Models.Entities.Membreship;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using static Qizilim.az.Models.Entities.Color;
using static Qizilim.az.Models.Entities.Images;

namespace Qizilim.az.Models.Entities
{
    public class Products : BaseEntity
    {
        public string Name { get; set; }
        public string Kateqoriya { get; set; }
        public string Nov { get; set; }
        public string aboutProduct { get; set; }
        public double Price { get; set; }
        public double Weight { get; set; }
        public bool HasDiamond { get; set; }
        public double CountDiamond { get; set; }
        public bool Delivery { get; set; }
        //premium product
        public bool PremiumProduct { get; set; }
        public DateTime? PremiumStartDate { get; set; }
        public DateTime? PremiumEndDate { get; set; }

        public bool IreliCekilmisProduct { get; set; }
        public DateTime? IreliStartDate { get; set; }
        public DateTime? IreliEndDate { get; set; }








        public virtual ICollection<ProductColors> ProductColor { get; set; }
        public virtual ICollection<ProductImage> ProductImages { get; set; }
        public virtual ICollection<ProductEyar> ProductEyar { get; set; }
        public virtual ICollection<ProductKateqoriya> ProductKateqoriya { get; set; }
        public virtual ICollection<LikedProduct> LikedProducts { get; set; }



        public class LikedProduct
        {
            public int UserId { get; set; }
            public virtual QizilimUser User { get; set; }
            public int ProductId { get; set; }
            public virtual Products Product { get; set; }
        }
    }
}
