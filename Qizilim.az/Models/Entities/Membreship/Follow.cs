using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Qizilim.az.Models.Entities.Membreship
{
    public class Follow
    {
        [Key]
        public int FollowerId { get; set; }
        public string FollowerName { get; set; }


        public virtual ICollection<FollowUser> FollowUser { get; set; }
    }


    public class FollowUser
    {
        public int FollowerId { get; set; }
        public virtual Follow Follower { get; set; }
        public int UserId { get; set; }
        public virtual QizilimUser User { get; set; }
    }
}
