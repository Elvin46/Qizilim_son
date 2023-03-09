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
    public class CentersSingleQuery : IRequest<Centers>
    {
        public int Id { get; set; }

        public class CentersSingleQueryHandler : IRequestHandler<CentersSingleQuery, Centers>
        {
            readonly QizilimDbContext db;
            public CentersSingleQueryHandler(QizilimDbContext db)
            {
                this.db = db;
            }
            public async Task<Centers> Handle(CentersSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.Centers
                    .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
