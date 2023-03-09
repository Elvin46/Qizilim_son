using Qizilim.az.Models.Entities.Membreship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Models.Entities.ViewModels
{
    public class MainViewModel
    {
        public List<Products> OxsarProducts { get; set; }
        public List<Products> Products { get; set; }
        public List<QizilimUser> QizilimUser { get; set; }
        public List<Advertisement> Advertisement { get; set; }
        public List<Color> Colors { get; set; }
        public List<Kateqoriya> Categories { get; set; }
        public List<Eyar> Eyars { get; set; }
        public List<Centers> Centers { get; set; }
        public List<QizilimUser> Shops { get; set; }

        public int? CategoryId { get; set; }
        public bool? hasDiamond { get; set; }
        public int? CenterId { get; set; }
        public int? EyarId { get; set; }
        public int? ColorId { get; set; }
        public bool? hasDelivery { get; set; }
        public double? minWeight { get; set; }
        public double? maxWeight { get; set; }
        public double? minPrice { get; set; }
        public double? maxPrice { get; set; }
    }
}
