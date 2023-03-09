using MediatR;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.CentersModule
{
    public class CentersAllQuery : IRequest<IEnumerable<Centers>>
    {
        public class CentersAllQueryHandler : IRequestHandler<CentersAllQuery, IEnumerable<Centers>>
        {
            private readonly QizilimDbContext db;

            public CentersAllQueryHandler(QizilimDbContext db)
            {
                this.db = db;
            }

            public async Task<IEnumerable<Centers>> Handle(CentersAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.Centers
                    .Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
