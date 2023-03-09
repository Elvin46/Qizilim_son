namespace Qizilim.az.Models.FormModels
{
    public class FilterViewModel
    {
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
