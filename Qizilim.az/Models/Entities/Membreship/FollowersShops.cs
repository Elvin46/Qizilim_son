using Qizilim.az.AppCode.InfraStructure;

namespace Qizilim.az.Models.Entities.Membreship
{
    public class FollowersShops : BaseEntity
    {
        public int FollowerId { get; set; }
        public virtual QizilimUser Follower { get; set; }
        public int ShopId { get; set; }
    }
}
