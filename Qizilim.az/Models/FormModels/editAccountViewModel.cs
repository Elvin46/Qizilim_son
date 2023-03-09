using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Models.FormModels
{
    public class editAccountViewModel
    {
        public int Id { get; set; }
        public string? shopName { get; set; }
        public string? Email { get; set; }
        public string? aboutShop { get; set; }
        public string? shopNumber { get; set; }
        public string? instagramLink { get; set; }
        public string? tiktokLink { get; set; }
        public double? whatsappNumber { get; set; }
        public string? shopLocation { get; set; }
        public bool catdirilma { get; set; }
        public string ProfileImg { get; set; }
        public IFormFile file { get; set; }
    }
}
