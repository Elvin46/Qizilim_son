using MediatR;
using Microsoft.EntityFrameworkCore;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.KateqoriyasModule
{
    public class KateqoriyasSingleQuery : IRequest<Kateqoriya>
    {
        public int Id { get; set; }

        public class KateqoriyasSingleQueryHandler : IRequestHandler<KateqoriyasSingleQuery, Kateqoriya>
        {
            private readonly QizilimDbContext db;

            public KateqoriyasSingleQueryHandler(QizilimDbContext db)
            {
                this.db = db;
            }
            public async Task<Kateqoriya> Handle(KateqoriyasSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.Kateqoriya
                    .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
