using MediatR;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.EyarModule
{
    public class EyarAllQuery : IRequest<IEnumerable<Eyar>>
    {
        public class EyarAllQueryHandler : IRequestHandler<EyarAllQuery, IEnumerable<Eyar>>
        {
            private readonly QizilimDbContext db;

            public EyarAllQueryHandler(QizilimDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<Eyar>> Handle(EyarAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.Eyars
                    .Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
