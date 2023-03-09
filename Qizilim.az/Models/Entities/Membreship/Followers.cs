using Qizilim.az.AppCode.InfraStructure;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Models.Entities.Membreship
{
    public class Followers : BaseEntity
    {
        public int FollowerId { get; set; }
        public string FollowerName { get; set; }


        public virtual ICollection<FollowUser> FollowUser { get; set; }
    }
    public class FollowerShop : BaseEntity
    {
        public int FollowerId { get; set; }
        public virtual QizilimUser Follower { get; set; }
        public int ShopId { get; set; }
        public virtual QizilimUser Shop { get; set; }
    }
}
