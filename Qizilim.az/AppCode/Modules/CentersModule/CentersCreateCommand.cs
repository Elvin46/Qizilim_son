using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Qizilim.az.AppCode.Extensions;
using Qizilim.az.Models.DataContext;
using Qizilim.az.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Qizilim.az.AppCode.Modules.CentersModule
{
    public class CentersCreateCommand : IRequest<Centers>
    {
        public string Name { get; set; }


        public class CentersCreateCommandHandler : IRequestHandler<CentersCreateCommand, Centers>
        {
            private readonly QizilimDbContext db;
            private readonly IActionContextAccessor ctx;
            public CentersCreateCommandHandler(QizilimDbContext db,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Centers> Handle(CentersCreateCommand request, CancellationToken cancellationToken)
            {
                var centers = new Centers();
                centers.Name = request.Name;
                centers.CreatedDate = DateTime.UtcNow.AddHours(4);
                centers.CreatedById = ctx.GetPrincipalId();

                await db.Centers.AddAsync(centers, cancellationToken);
                await db.SaveChangesAsync(cancellationToken);

                return centers;
            }
        }
    }
}
