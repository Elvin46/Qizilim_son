using MediatR;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.KateqoriyasModule
{
    public class KateqoriyasAllQuery : IRequest<IEnumerable<Kateqoriya>>
    {
        public class KateqoriyasAllQueryHandler : IRequestHandler<KateqoriyasAllQuery, IEnumerable<Kateqoriya>>
        {
            private readonly QizilimDbContext db;

            public KateqoriyasAllQueryHandler(QizilimDbContext db)
            {
                this.db = db;
            }
            public async Task<IEnumerable<Kateqoriya>> Handle(KateqoriyasAllQuery request, CancellationToken cancellationToken)
            {
                var model = await db.Kateqoriya.Where(ah => ah.DeletedById == null).ToListAsync(cancellationToken);

                return model;
            }
        }
    }
}
