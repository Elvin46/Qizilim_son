using MediatR;
using Microsoft.AspNetCore.Mvc.Infrastructure;
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
    public class CentersEditCommand : IRequest<Centers>
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public class CentersEditCommandHandler : IRequestHandler<CentersEditCommand, Centers>
        {
            readonly QizilimDbContext db;
            readonly IActionContextAccessor ctx;
            public CentersEditCommandHandler(QizilimDbContext db,
                IActionContextAccessor ctx)
            {
                this.db = db;
                this.ctx = ctx;
            }
            public async Task<Centers> Handle(CentersEditCommand request, CancellationToken cancellationToken)
            {
                var entity = await db.Centers
                       .FirstOrDefaultAsync(b => b.Id == request.Id && b.DeletedById == null, cancellationToken);

                if (entity == null)
                {
                    return null;
                }

                entity.Name = request.Name;
                await db.SaveChangesAsync(cancellationToken);

                return entity;
            }
        }
    }
}
