using MediatR;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.ColorsModule
{
    public class ColorsAllQuery : IRequest<IEnumerable<Color>>
    {

        public class ColorsAllQueryHandler : IRequestHandler<ColorsAllQuery, IEnumerable<Color>>
        {
            private readonly QizilimDbContext db;

            public ColorsAllQueryHandler(QizilimDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<Color>> Handle(ColorsAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.Colors
                    .Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
