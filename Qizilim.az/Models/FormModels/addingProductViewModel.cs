using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Models.FormModels
{
    public class addingProductViewModel
    {
        //arrays
        [Required]
        public IFormFile[]? files { get; set; }
        public string[]? ProductImagesPath { get; set; }
        [Required]
        public string? Kateqoriya { get; set; }
        [Required]
        public int ColorsId { get; set; }
        [Required]
        public int EyarOlcusuId { get; set; }


        [Required]
        public string? Name { get; set; }
        public string? aboutProduct { get; set; }
        [Required]
        public double Price { get; set; }
        [Required]
        public double Weight { get; set; }
        [Required]
        public bool HasDiamond { get; set; }
        public double CountDiamond { get; set; }
    }
}
