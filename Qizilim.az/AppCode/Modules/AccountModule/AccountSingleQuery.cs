using MediatR;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities.Membreship;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.AccountModule
{
    public class AccountSingleQuery : IRequest<QizilimUser>
    {
        public int Id { get; set; }

        public class AccountSingleQueryHandler : IRequestHandler<AccountSingleQuery, QizilimUser>
        {
            private readonly QizilimDbContext db;

            public AccountSingleQueryHandler(QizilimDbContext db)
            {
                this.db = db;
            }
            public async Task<QizilimUser> Handle(AccountSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.Users
                      .FirstOrDefaultAsync(b => b.Id == request.Id, cancellationToken);

                return model;
            }
        }
    }
}
