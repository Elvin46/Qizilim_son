using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using static Qizilim.az.Models.Entities.Products;

namespace Qizilim.az.Models.Entities.Membreship
{
    public class QizilimUser : IdentityUser<int>
    {
        [Required]
        public string Name { get; set; }
        [Required]
        public string Surname { get; set; }
        public string ProfileImg { get; set; }

        public double? Wallet { get; set; }

        //registerVerify
        public bool? Status { get; set; }


        //magazalarucun

        public string? shopName { get; set; }
        public string? aboutShop { get; set; }
        public string? shopNumber { get; set; }
        public string? instagramLink { get; set; }
        public string? tiktokLink { get; set; }
        public double? whatsappNumber { get; set; }
        public string? shopLocation { get; set; }
        public bool catdirilma { get; set; }
        public ICollection<FollowersShops> FollowerShops { get; set; }
        public int FollowerCount { get; set; }


        public virtual ICollection<LikedProduct> LikedProducts { get; set; }
        public virtual ICollection<FollowUser> FollowUser { get; set; }
    }
}
