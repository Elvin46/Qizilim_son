using MediatR;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities.Membreship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.RegisterVerify
{
    public class UsersAllQuery : IRequest<IEnumerable<QizilimUser>>
    {

        public class UsersAllQueryHandler : IRequestHandler<UsersAllQuery, IEnumerable<QizilimUser>>
        {
            readonly private QizilimDbContext db;
            public UsersAllQueryHandler(QizilimDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<QizilimUser>> Handle(UsersAllQuery request, CancellationToken cancellationToken)
            {
                var entity = await db.Users.Where(u => u.shopName != null).ToListAsync();


                return entity;
            }
        }
    }
}
