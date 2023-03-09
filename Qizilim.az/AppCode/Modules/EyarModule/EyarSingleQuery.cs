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
    public class EyarSingleQuery : IRequest<Eyar>
    {
        public int Id { get; set; }


        public class EyarSingleQueryHandler : IRequestHandler<EyarSingleQuery, Eyar>
        {
            readonly QizilimDbContext db;
            public EyarSingleQueryHandler(QizilimDbContext db)
            {
                this.db = db;
            }
            public async Task<Eyar> Handle(EyarSingleQuery request, CancellationToken cancellationToken)
            {
                var model = await db.Eyars
                    .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                return model;
            }
        }
    }
}
